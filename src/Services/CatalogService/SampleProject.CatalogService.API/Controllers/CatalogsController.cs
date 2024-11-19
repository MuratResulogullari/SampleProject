
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SampleProject.CatalogService.API.Contexts;
using SampleProject.CatalogService.API.Entities;
using SampleProject.CatalogService.API.SeedWork;
using System.Text;
using System.Text.Json;

namespace SampleProject.CatalogService.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CatalogsController : ControllerBase
  {
    public CatalogDbContext _context;

    public IOptions<CatalogOptions> _options;

    public ILogger<CatalogsController> _logger;

    public IDistributedCache _cache;
    public IConnection _connection;

    public CatalogsController(CatalogDbContext context, IOptions<CatalogOptions> options, ILogger<CatalogsController> logger, IDistributedCache cache,IConnection connection)
    {
      _context = context;
      _options = options;
      _logger = logger;
      _cache = cache;
      _connection = connection;
    }
    [HttpGet("GetAllItems")]
    public  async Task<IActionResult> GetAllItemsAsync([FromQuery] int pageSize,[FromQuery] int pageIndex)
    {
      //var pageSize = paginationRequest.PageSize;
      //var pageIndex = paginationRequest.PageIndex;

      long catalogsCount = await _context.Catalogs.LongCountAsync();
      var catalogCountMessage = new CatalogCountMessage { CatalogCount = catalogsCount };
      // Send a message to the queue in RabbitMQ
      var channel = _connection.CreateModel();
      channel.QueueDeclare(queue: "CatalogServiceQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
   
      var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(catalogCountMessage));

      channel.BasicPublish(exchange: string.Empty, routingKey: "CatalogServiceQueue", basicProperties: null, body: body);

      // Check it there are cached items
      var cachedItems = await _cache.GetAsync("catalogs");

      if (cachedItems is null)
      {
        // There are no items in the cache. Get them from the database
        var itemsOnPage = await _context.Catalogs
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        // Store the items in the cache for 10 seconds
        await _cache.SetAsync("catalogs", Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(itemsOnPage)), new()
        {
          AbsoluteExpiration = DateTime.Now.AddDays(10)
        });

        ChangeUriPlaceholder(_options.Value, itemsOnPage);
        return Ok(new PaginatedItems<Catalog>(pageIndex, pageSize, catalogsCount, itemsOnPage));

      }
      else
      {
        // There are items in the cache. Deserialize them to display.
        var itemsOnPage = System.Text.Json.JsonSerializer.Deserialize<List<Catalog>>(cachedItems);
        // Make sure itemsOnPage is not null
        if (itemsOnPage is null)
        {
          itemsOnPage = new List<Catalog>();
        }

        ChangeUriPlaceholder(_options.Value, itemsOnPage);
        return Ok(new PaginatedItems<Catalog>(pageIndex, pageSize, catalogsCount, itemsOnPage));
      }

    }
    [HttpGet("get-catalogs-on-cache")]
    public async Task<IActionResult> GetCatalogOnCache()
    {
      var result = new List<Catalog> ();
      var cachedItems = await _cache.GetAsync("catalogs");
      if (cachedItems is not null)
      {
        result = System.Text.Json.JsonSerializer.Deserialize<List<Catalog>>(cachedItems);
      }
      return Ok(result);
    }
      private static void ChangeUriPlaceholder(CatalogOptions options, List<Catalog> items)
    {
      foreach (var item in items)
      {
        item.PictureUri = options.GetPictureUrl(item.Id);
      }
    }
    public async Task<List<T>> RetrieveListAsync<T>(string key)
{
    // Get the serialized list from the cache
    var serializedList = await _cache.GetStringAsync(key);

    if (serializedList != null)
    {
        // Deserialize the JSON string to a list
        return JsonSerializer.Deserialize<List<T>>(serializedList);
    }

    // Return an empty list or handle it according to your needs
    return new List<T>();
}
  }
  public class CatalogCountMessage
  {
   public long CatalogCount { get; set; }
  }
}

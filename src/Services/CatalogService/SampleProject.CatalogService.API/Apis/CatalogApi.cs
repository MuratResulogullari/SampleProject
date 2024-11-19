using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleProject.CatalogService.API.Contexts;
using SampleProject.CatalogService.API.Entities;
using SampleProject.CatalogService.API.SeedWork;
using SampleProject.CatalogService.API.Services;

namespace SampleProject.CatalogService.API.Apis;

//public static class CatalogApi
//{
//  public static IEndpointRouteBuilder MapCatalogApi(this IEndpointRouteBuilder app)
//  {
//    // Routes for querying catalog items.
//    app.MapGet("/items", GetAllItemsAsync);
//    app.MapGet("/catalogtypes", async (CatalogDbContext context) => await context.CatalogTypes.OrderBy(x => x.Type).AsNoTracking().ToListAsync());

//    return app;
//  }
//  public static async Task<Results<Ok<PaginatedItems<Catalog>>,BadRequest<string>>> GetAllItemsAsync(
//      PaginationRequest paginationRequest , 
//      CatalogApiService service,
//      IDistributedCache cache,
//      IConnection connection)
//  {
//    var pageSize = paginationRequest.PageSize;
//    var pageIndex = paginationRequest.PageIndex;

//    var catalogsCount = await service.DbContext.Catalogs.LongCountAsync();

//    // Send a message to the queue in RabbitMQ
//    //var channel = connection.CreateModel();
//    //channel.QueueDeclare(queue: "catalogEvents",
//    //                 durable: false,
//    //                 exclusive: false,
//    //                 autoDelete: false,
//    //                 arguments: null);
//    //var body = Encoding.UTF8.GetBytes("Getting all items in the catalog.");

//    //channel.BasicPublish(exchange: string.Empty,
//    //                     routingKey: "catalogEvents",
//    //                     basicProperties: null,
//    //                     body: body);

//    // Check it there are cached items
//    var cachedItems = await cache.GetAsync("catalogItems");

//    if (cachedItems is null)
//    {
//      // There are no items in the cache. Get them from the database
//      var itemsOnPage = await service.DbContext.Catalogs
//          .OrderBy(c => c.Name)
//          .Skip(pageSize * pageIndex)
//          .Take(pageSize)
//          .AsNoTracking()
//          .ToListAsync();

//      // Store the items in the cache for 10 seconds
//      await cache.SetAsync("catalogItems", Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(itemsOnPage)), new()
//      {
//        AbsoluteExpiration = DateTime.Now.AddSeconds(10)
//      });

//      ChangeUriPlaceholder(service.Options.Value, itemsOnPage);
//      return TypedResults.Ok(new PaginatedItems<Catalog>(pageIndex, pageSize, catalogsCount, itemsOnPage));

//    }
//    else
//    {
//      // There are items in the cache. Deserialize them to display.
//      var itemsOnPage = System.Text.Json.JsonSerializer.Deserialize<List<Catalog>>(cachedItems);
//      // Make sure itemsOnPage is not null
//      if (itemsOnPage is null)
//      {
//        itemsOnPage = new List<Catalog>();
//      }

//      ChangeUriPlaceholder(service.Options.Value, itemsOnPage);
//      return TypedResults.Ok(new PaginatedItems<Catalog>(pageIndex, pageSize, catalogsCount, itemsOnPage));
//    }
//  }
//  private static void ChangeUriPlaceholder(CatalogOptions options, List<Catalog> items)
//  {
//    foreach (var item in items)
//    {
//      item.PictureUri = options.GetPictureUrl(item.Id);
//    }
//  }

//}

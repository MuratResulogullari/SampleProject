using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SampleProject.CatalogService.API.Contexts;

namespace SampleProject.CatalogService.API.Services
{
  public readonly struct CatalogApiService(CatalogDbContext context,IOptions<CatalogOptions> options, ILogger<CatalogApiService> logger)
  {
    public CatalogDbContext DbContext { get; } = context;

    public IOptions<CatalogOptions> Options { get; } = options;

    public ILogger<CatalogApiService> Logger { get; } = logger;
  }
}

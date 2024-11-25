using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SampleProject.CatalogService.API.Contexts;
using SampleProject.CatalogService.API.Services;

namespace SampleProject.CatalogService.API
{
  public static class CatalogServiceExtensions
  {
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
     
      builder.AddNpgsqlDbContext<CatalogDbContext>("CatalogDB", null,
      optionsBuilder => optionsBuilder.UseNpgsql(npgsqlBuilder => npgsqlBuilder.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));
      
      builder.Services.AddMigration<CatalogDbContext, CatalogContextSeed>();

      builder.Services.Configure<CatalogOptions>(builder.Configuration.GetSection(nameof(CatalogOptions)));
      
     

    }

    public static TOptions GetOptions<TOptions>(this IHost host)  where TOptions : class, new()
    {
      return host.Services.GetRequiredService<IOptions<TOptions>>().Value;
    }
  }
}

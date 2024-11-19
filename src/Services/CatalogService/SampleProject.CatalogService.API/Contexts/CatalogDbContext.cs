using Microsoft.EntityFrameworkCore;
using SampleProject.CatalogService.API.Entities;
using SampleProject.CatalogService.API.EntityConfigurations;

namespace SampleProject.CatalogService.API.Contexts
{
  /// <remarks>
  /// Add migrations using the following command inside the 'SampleProject.CatalogService.API' project directory:
  ///
  /// dotnet ef migrations add --context CatalogDbContext InitialMigration
  /// </remarks>
  public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
  {
    public DbSet<Catalog> Catalogs{ get; set; }

    public DbSet<CatalogBrand> CatalogBrands { get; set; }

    public DbSet<CatalogType> CatalogTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      //modelBuilder.HasDefaultSchema("catalog");
      base.OnModelCreating(modelBuilder);
      modelBuilder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
      modelBuilder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
      modelBuilder.ApplyConfiguration(new CatalogEntityTypeConfiguration());
    }
  }
}

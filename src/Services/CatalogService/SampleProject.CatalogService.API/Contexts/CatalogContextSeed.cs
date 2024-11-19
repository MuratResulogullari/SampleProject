using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SampleProject.CatalogService.API.Entities;

namespace SampleProject.CatalogService.API.Contexts;

public partial class CatalogContextSeed(IWebHostEnvironment env, ILogger<CatalogContextSeed> logger)  : IDbSeeder<CatalogDbContext>
{
    public async Task SeedAsync(CatalogDbContext context)
    {
        var contentRootPath = env.ContentRootPath;
        var picturePath = env.WebRootPath;

   
        context.Database.OpenConnection();
        ((NpgsqlConnection)context.Database.GetDbConnection()).ReloadTypes();

        if (!context.Catalogs.Any())
        {
            var sourcePath = Path.Combine(contentRootPath,"Setup", "catalog.json");
            var sourceJson = File.ReadAllText(sourcePath);
            var sourceItems = JsonSerializer.Deserialize<CatalogSourceEntry[]>(sourceJson)
                ?? throw new InvalidOperationException($"Seed data file was found but it contained no data: '{sourcePath}'");

            context.CatalogBrands.RemoveRange(context.CatalogBrands);
            await context.CatalogBrands.AddRangeAsync(sourceItems.Select(x => x.Brand).Distinct()
                .Select(brandName => new CatalogBrand { Brand = brandName }));
            logger.LogInformation("Seeded catalog with {NumBrands} brands", context.CatalogBrands.Count());

            context.CatalogTypes.RemoveRange(context.CatalogTypes);
            await context.CatalogTypes.AddRangeAsync(sourceItems.Select(x => x.Type).Distinct()
                .Select(typeName => new CatalogType { Type = typeName }));
            logger.LogInformation("Seeded catalog with {NumTypes} types", context.CatalogTypes.Count());

            await context.SaveChangesAsync();

            var brandIdsByName = await context.CatalogBrands.ToDictionaryAsync(x => x.Brand, x => x.Id);
            var typeIdsByName = await context.CatalogTypes.ToDictionaryAsync(x => x.Type, x => x.Id);

            await context.Catalogs.AddRangeAsync(sourceItems.Select(source => new Catalog
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                Price = source.Price,
                CatalogBrandId = brandIdsByName[source.Brand],
                CatalogTypeId = typeIdsByName[source.Type],
                AvailableStock = 100,
                MaxStockThreshold = 200,
                RestockThreshold = 10,
                PictureFileName = $"{source.Id}.webp"
            }));

            logger.LogInformation("Seeded catalog with {NumItems} items", context.Catalogs.Count());

            await context.SaveChangesAsync();
        }
    }

    private class CatalogSourceEntry
    {
        public int Id { get; set; }
        public required string Type { get; set; }
        public required string Brand { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
    }
}

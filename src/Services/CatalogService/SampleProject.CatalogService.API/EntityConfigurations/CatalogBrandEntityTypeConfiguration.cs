using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleProject.CatalogService.API.Entities;

namespace SampleProject.CatalogService.API.EntityConfigurations;

internal class CatalogBrandEntityTypeConfiguration : IEntityTypeConfiguration<CatalogBrand>
{
    public void Configure(EntityTypeBuilder<CatalogBrand> builder)
    {
        builder.ToTable("CatalogBrands", "catalog");

        builder.Property(cb => cb.Brand)
            .HasMaxLength(100);
    }
}

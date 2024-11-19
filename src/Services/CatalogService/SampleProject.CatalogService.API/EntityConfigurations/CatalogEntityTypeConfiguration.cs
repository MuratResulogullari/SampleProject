using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleProject.CatalogService.API.Entities;

namespace SampleProject.CatalogService.API.EntityConfigurations;

internal class CatalogEntityTypeConfiguration : IEntityTypeConfiguration<Catalog>
{
    public void Configure(EntityTypeBuilder<Catalog> builder)
    {
        builder.ToTable("Catalogs", "catalog");

        builder.Property(ci => ci.Name)
            .HasMaxLength(50);

        builder.Ignore(ci => ci.PictureUri);

        builder.HasOne(ci => ci.CatalogBrand)
            .WithMany();

        builder.HasOne(ci => ci.CatalogType)
            .WithMany();

        builder.HasIndex(ci => ci.Name);
    }
}

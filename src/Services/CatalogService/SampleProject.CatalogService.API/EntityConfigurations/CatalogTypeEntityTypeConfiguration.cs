using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleProject.CatalogService.API.Entities;

namespace SampleProject.CatalogService.API.EntityConfigurations;

internal class CatalogTypeEntityTypeConfiguration: IEntityTypeConfiguration<CatalogType>
{
    public void Configure(EntityTypeBuilder<CatalogType> builder)
    {
        builder.ToTable("CatalogType", "catalog");

        builder.Property(cb => cb.Type)
            .HasMaxLength(100);
    }
}

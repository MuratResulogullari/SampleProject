using System.ComponentModel.DataAnnotations;

namespace SampleProject.CatalogService.API.Entities
{
  public class CatalogBrand
  {
    public int Id { get; set; }

    [Required]
    public required string Brand { get; set; }
  }
}

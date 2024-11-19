using System.ComponentModel.DataAnnotations;

namespace SampleProject.CatalogService.API.Entities
{
  public class CatalogType
  {
    public int Id { get; set; }

    [Required]
    public required string Type { get; set; }
  }
}

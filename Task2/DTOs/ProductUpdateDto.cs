using System.ComponentModel.DataAnnotations;

namespace Task2.DTOs;

public class ProductUpdateDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
}

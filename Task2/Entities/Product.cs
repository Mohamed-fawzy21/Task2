using System.ComponentModel.DataAnnotations;

namespace Task2.Entities;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public bool IsDeleted { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
}

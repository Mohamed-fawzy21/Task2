using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task2.Data;
using Task2.DTOs;

namespace Task2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(AppDbContext dbContext) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("pageNumber and pageSize must be greater than zero.");
        }

        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(product => product.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(product => new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryName = product.Category!.Name
            })
            .ToListAsync();

        return Ok(products);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto updateDto)
    {
        var product = await dbContext.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        product.Name = updateDto.Name;
        product.Price = updateDto.Price;

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict("The product was changed by another request. Reload it and try again.");
        }

        return NoContent();
    }
}

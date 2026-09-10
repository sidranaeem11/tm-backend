using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    private const string AdminKey = "nokia3310";

    public ProductsController(AppDbContext context) => _context = context;

    private bool IsAuthorized() =>
        Request.Headers.TryGetValue("X-Admin-Key", out var key) && key == AdminKey;

    // ✅ GetProducts - Include Category
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products
            .Include(p => p.Category)    // ✅ Yeh zaroori hai!
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return NotFound();
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        if (!IsAuthorized()) return Unauthorized();
        
        var category = await _context.Categories.FindAsync(product.CategoryId);
        if (category == null)
        {
            return BadRequest(new { error = $"Category with ID {product.CategoryId} not found" });
        }

        product.Category = null;
        
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        
        await _context.Entry(product).Reference(p => p.Category).LoadAsync();
        
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        if (!IsAuthorized()) return Unauthorized();
        if (id != product.Id) return BadRequest();
        
        var existing = await _context.Products.FindAsync(id);
        if (existing == null) return NotFound();
        
        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.OldPrice = product.OldPrice;
        existing.ImageUrl = product.ImageUrl;
        existing.Size = product.Size;
        existing.Color = product.Color;
        existing.CategoryId = product.CategoryId;
        existing.IsNewArrival = product.IsNewArrival;
        existing.IsBestSeller = product.IsBestSeller;
        existing.Stock = product.Stock;
        existing.GalleryImages = product.GalleryImages;
        existing.IsB2B = product.IsB2B;
        existing.IsB2C = product.IsB2C;
        existing.BulkPrice = product.BulkPrice;
        existing.MinOrderQuantity = product.MinOrderQuantity;
        existing.WholesalePrice = product.WholesalePrice;
        
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        if (!IsAuthorized()) return Unauthorized();
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
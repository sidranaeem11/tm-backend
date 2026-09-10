using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private const string AdminKey = "nokia3310";

    private bool IsAuthorized() =>
        Request.Headers.TryGetValue("X-Admin-Key", out var key) && key == AdminKey;

    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile image)
    {
        if (!IsAuthorized()) return Unauthorized();
        if (image == null || image.Length == 0) return BadRequest("No file uploaded.");

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        var url = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
        return Ok(new { url });
    }
}
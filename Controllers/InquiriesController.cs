using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InquiryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InquiryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInquiry([FromBody] Inquiry inquiry)
        {
            try
            {
                if (inquiry == null)
                {
                    return BadRequest("Invalid inquiry data");
                }

                inquiry.CreatedAt = DateTime.UtcNow;
                _context.Inquiries.Add(inquiry);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Inquiry submitted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetInquiries()
        {
            try
            {
                var inquiries = await _context.Inquiries.ToListAsync();
                return Ok(inquiries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
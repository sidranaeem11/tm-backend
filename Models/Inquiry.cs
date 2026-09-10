using System;

namespace backend.Models
{
    public class Inquiry
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public string ProductInterest { get; set; }
        public string Quantity { get; set; }
        public string Customization { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public bool IsNewArrival { get; set; }
        public bool IsBestSeller { get; set; }
        public int Stock { get; set; }
        public string[] GalleryImages { get; set; } = Array.Empty<string>();

        // ✅ B2B / B2C Fields
        public bool IsB2B { get; set; } = false;
        public bool IsB2C { get; set; } = true;
        public decimal? BulkPrice { get; set; }
        public int? MinOrderQuantity { get; set; } = 1;
        public decimal? WholesalePrice { get; set; }
    }
}
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string ImageUrl { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }
}
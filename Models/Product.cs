using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPICoreDapper.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Sku { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public float Price { get; set; }

        public float? DiscountPrice { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int ViewCount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
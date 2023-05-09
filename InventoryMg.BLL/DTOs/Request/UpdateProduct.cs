using InventoryMg.DAL.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMg.BLL.DTOs.Request
{
    public class UpdateProduct
    {

        [Required, StringLength(50, ErrorMessage = "Product name should be between 5 to 50 characters", MinimumLength = 5)]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please Select a valid category")]
        public Category Category { get; set; }
        [Required]
        public long Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required, StringLength(50, ErrorMessage = "Product brandname should be between 5 to 50 characters", MinimumLength = 2)]
        public string BrandName { get; set; }
        public IFormFile? File { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace InventoryMg.BLL.DTOs.Request
{
    public class UpdateUser
    {
        [Required(ErrorMessage = "Fullname is required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

    }
}

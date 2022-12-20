using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Books.Mvc.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<SelectListItem> RoleList { get; set; }
        public string RoleSelected { get; set; }
    }


    public class RegisterationResponsetDto
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }

        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}

using System.ComponentModel.DataAnnotations;
using DatabaseContext.Models;
using Model.Database;

namespace CRMM.Models
{
    public class LoginModel
    {
        [Required]
        [MinLength(1)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Heslo")]
        public string Password { get; set; }
    }
    public class ApiLoginModel
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Heslo")]
        public string Password { get; set; }
    }
}
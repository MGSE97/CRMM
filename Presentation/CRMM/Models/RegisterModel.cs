using System.ComponentModel.DataAnnotations;
using DatabaseContext.Models;
using Model.Database;

namespace CRMM.Models
{
    public class RegisterModel
    {
        [Key] public ulong Id { get; set; }

        [Required]
        [MinLength(1)]
        [Display(Name = "Jméno")]
        public string Name { get; set; }

        [Required]
        [MinLength(1)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Heslo")]
        public string Password { get; set; }
    }
}
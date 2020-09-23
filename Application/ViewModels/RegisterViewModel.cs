using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class RegisterViewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Hasło musi mieć co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasła nie są takie same.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Klucz")]
        public string Key { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Nazwa firmy")]
        public string CompanyName { get; set; }
    }
}


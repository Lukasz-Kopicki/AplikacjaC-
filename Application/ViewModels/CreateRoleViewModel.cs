using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Nazwa profilu")]
        public string RoleName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }

        [DisplayName("ID profilu")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Nazwa profilu nie może być pusta.")]
        [DisplayName("Nazwa profilu")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}

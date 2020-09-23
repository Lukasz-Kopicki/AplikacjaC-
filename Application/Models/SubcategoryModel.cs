using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models
{
    public class SubcategoryModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Pole nie może być puste")]
        [DisplayName("Nazwa podkategorii")]
        [MaxLength(200)]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public CategoryModel Category { get; set; }
    }
}

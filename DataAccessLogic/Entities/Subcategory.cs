using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLogic.Entities
{
    public class Subcategory
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Pole nie może być puste")]
        [DisplayName("Nazwa podkategorii")]
        [MaxLength(200)]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

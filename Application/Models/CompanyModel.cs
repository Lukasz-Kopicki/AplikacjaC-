using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models
{
    public class CompanyModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string PhotoFilePath { get; set; }
        public AddressModel Address { get; set; }
        public CityModel City { get; set; }
        public List<WorkplaceModel> Workplaces { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models
{
    public class AddressModel
    {
        public int Id { get; set; }
        [Required]
        public CityModel City { get; set; } 

        [Required]
        [MaxLength(30)]
        [Column(TypeName = "varchar(30)")]
        public string PostalCode { get; set; }

        [MaxLength(50)]
        public string Street { get; set; }
        [MaxLength(10)]
        public string StreetNumber { get; set; }
        [MaxLength(10)]
        public string ApartmentNumber { get; set; }
    }
}

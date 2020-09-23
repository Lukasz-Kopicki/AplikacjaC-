using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLogic.Entities
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string PhotoFilePath { get; set; }
        public Address Address { get; set; }
        public City City { get; set; }
        public List<Workplace> Workplaces { get; set; }

    }
}

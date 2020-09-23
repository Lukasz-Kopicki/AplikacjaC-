using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLogic.Entities
{
    public class Worker
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(200)]
        public string PhotoFilePath { get; set; }
        public Workplace Workplace { get; set; }
        public List<Service> Services { get; set; }
    }
}

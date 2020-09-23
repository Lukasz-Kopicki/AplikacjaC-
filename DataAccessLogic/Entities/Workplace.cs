using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLogic.Entities
{
    public class Workplace
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string PhotoFilePath { get; set; }
        public Company Company { get; set; }
        public Address Address { get; set; }
        public List<Worker> Workers { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class WorkplaceModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string PhotoFilePath { get; set; }
        public CompanyModel Company { get; set; }
        public AddressModel Address { get; set; }
        public List<WorkerModel> Workers { get; set; }
    }
}

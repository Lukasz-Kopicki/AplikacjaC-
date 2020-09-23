using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models
{
    public class WorkerModel
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
        public WorkplaceModel Workplace { get; set; }
        public List<ServiceModel> Services { get; set; }
    }
}

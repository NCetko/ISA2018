using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.Entities
{
    public class RAC
    {
        public RAC() {
            Provider = new Provider();
            Ratable = new Ratable();
        }

        [Key]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string RACName { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Required")]
        public Ratable Ratable { get; set; }

        [Required(ErrorMessage = "Required")]
        public Provider Provider { get; set; }

        [Required(ErrorMessage = "Required")]
        public Destination Destination { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<RACOffice> RACOffices { get; set; }


    }

}
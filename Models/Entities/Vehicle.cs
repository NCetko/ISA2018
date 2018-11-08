using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ISA.Enums;

namespace ISA.Models.Entities
{
    public class Vehicle
    {
        public Vehicle() {
            Ratable = new Ratable();
        }

        public string RACName { get; set; }

        [Required(ErrorMessage = "Required")]
        [ForeignKey("RACName")]
        public RAC RAC { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Name")]
        public string VehicleName { get; set; }

        [Required(ErrorMessage = "Required")]
        public int Passengers { get; set; }

        [Display(Name = "Trunc Volume")]
        [Required(ErrorMessage = "Required")]
        public int TruncVolume { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue)]
        public int Price { get; set; }

        [Required(ErrorMessage = "Required")]
        public int Doors { get; set; }

        public byte[] Image { get; set; }

        public Ratable Ratable { get; set; }

    }

}

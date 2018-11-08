using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ISA.Enums;

namespace ISA.Models.Entities
{
    public class VehicleDiscount
    {
        public VehicleDiscount() { }

        [Key]
        public int VehicleDiscountId { get; set; }

        [Required(ErrorMessage = "Required")]
        public Vehicle Vehicle { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue)]
        public int Price { get; set; }

        public  Reservation Reservation { get; set; }

    }

}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ISA.Enums;

namespace ISA.Models.Entities
{
    public class VehicleReservation
    {
        public VehicleReservation() { }

        [Key]
        public int VehicleReservationId { get; set; }

        [Required(ErrorMessage = "Required")]
        public Vehicle Vehicle { get; set; }

        [Required(ErrorMessage = "Required")]
        public Reservation Reservation { get; set; }

    }

}

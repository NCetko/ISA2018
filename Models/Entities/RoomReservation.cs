using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ISA.Enums;

namespace ISA.Models.Entities
{
    public class RoomReservation
    {
        public RoomReservation() { }

        [Key]
        public int RoomReservationId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Column(TypeName = "date")]
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Required")]
        [Column(TypeName = "date")]
        [Display(Name = "End date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Required")]
        public RoomPrice RoomPrice { get; set; }

        public Reservation Reservation { get; set; }
    }

}

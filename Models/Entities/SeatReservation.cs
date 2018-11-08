using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ISA.Enums;

namespace ISA.Models.Entities
{
    public class SeatReservation
    {
        public SeatReservation() { }

        [Key]
        public int SeatReservationId { get; set; }

        [Required(ErrorMessage = "Required")]
        public Seat Seat { get; set; }

        [Required(ErrorMessage = "Required")]
        public Flight Flight { get; set; }

        [Required(ErrorMessage = "Required")]
        public Reservation Reservation { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Passport { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public bool Confirmed { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }

}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISA.Models.Entities
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        [Required(ErrorMessage = "Required")]
        public DateTime Created { get; set; }

        [Required(ErrorMessage = "Required")]
        public ApplicationUser ApplicationUser { get; set; }

        [Required(ErrorMessage = "Required")]
        public Airline Airline { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Total price")]
        public float TotalPrice { get; set; }

        //public  ICollection<RoomReservation> RoomReservations { get; set; }
        public  ICollection<SeatReservation> SeatReservations { get; set; }
        public  ICollection<Rating> Ratings { get; set; }

        //public  ICollection<VehicleReservation> VehicleReservations { get; set; }
    }
}

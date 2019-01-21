using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.Entities
{
    public class Flight
    {
        public Flight() {
            Ratable = new Ratable();
        }

        [Required(ErrorMessage = "Required")]
        [Column(TypeName = "datetime")]
        public DateTime Departure { get; set; }

        [Required(ErrorMessage = "Required")]
        [Column(TypeName = "datetime")]
        public DateTime Arrival { get; set; }

        [Key]
        [Display(Name = "Name")]
        public string FlightName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue)]
        public int Price { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue)]
        public int KM { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Carry-On Bag")]
        public int CarryOnBag { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Checked Bag")]
        public int CheckedBag { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Departure Location")]
        public AirlineDestination DepartureLocation { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Arrival Location")]
        public AirlineDestination ArrivalLocation { get; set; }

        [Display(Name = "Baggage Details")]
        public string BaggageDetails { get; set; }

        public string Services { get; set; }

        [Required(ErrorMessage = "Required")]
        public Airplane Airplane { get; set; }

        public Ratable Ratable { get; set; }

        public ICollection<SeatDiscount> SeatDiscounts { get; set; }
        public ICollection<SeatReservation> SeatReservations { get; set; }
    }

}
using ISA.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.FlightViewModels
{
    public class CreateViewModel
    {
        [Display(Name = "Flight Name")]
        public string FlightName { get; set; }

        [Required(ErrorMessage = "Required")]
        public DateTime Departure { get; set; }

        [Required(ErrorMessage = "Required")]
        public DateTime Arrival { get; set; }

        [Display(Name = "Preceding Flight")]
        public string PrecedingFlightName { get; set; }

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
        [Display(Name = "Origin")]
        public string DepartureLocationName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Destination")]
        public string ArrivalLocationName { get; set; }


        [Display(Name = "Baggage Details")]
        public string BaggageDetails { get; set; }

        public string Services { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Airplane Name")]
        public string AirplaneName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Airline Name")]
        public string AirlineName { get; set; }
    }
}

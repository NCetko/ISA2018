﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.Entities
{
    public class AirlineDestination
    {
        public AirlineDestination() { }

        [Display(Name = "Airline Name")]
        public string AirlineName { get; set; }
        
        [Display(Name = "Destination Name")]
        public string DestinationName { get; set; }

        public bool Removed { get; set; }

        [ForeignKey("AirlineName")]
        public Airline Airline { get; set; }

        [ForeignKey("DestinationName")]
        public Destination Destination { get; set; }

        public ICollection<Flight> Arrivals { get; set; }
        public ICollection<Flight> Departures { get; set; }
    }

}
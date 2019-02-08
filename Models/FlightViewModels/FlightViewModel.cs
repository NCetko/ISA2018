using ISA.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.FlightViewModels
{
    public class FlightViewModel
    {
        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }

        public int Price { get; set; }

        public List<Flight> Flights { get; set; }

        public FlightViewModel()
        {
            Flights = new List<Flight>();
        }
    }
}

using ISA.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.FlightViewModels
{
    public class FilterViewModel
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        public int? MaxStops { get; set; }
    }
}

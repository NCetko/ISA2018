using ISA.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISA.Models.SeatReservationViewModels
{
    public class CreateViewModel
    {
        //(string passport, string firstName, string lastName, string flightName, string seatName, string segmentName)
        public string Passport { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FlightName { get; set; }
        public string SeatName { get; set; }
        public string SegmentName { get; set; }
    }
}

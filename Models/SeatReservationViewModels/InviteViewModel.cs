using ISA.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISA.Models.SeatReservationViewModels
{
    public class InviteViewModel
    {
        public string FlightName { get; set; }
        public string SeatName { get; set; }
        public string SegmentName { get; set; }
        public string FriendId { get; set; }
    }
}

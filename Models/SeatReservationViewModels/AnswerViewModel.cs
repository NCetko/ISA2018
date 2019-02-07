using ISA.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISA.Models.SeatReservationViewModels
{
    public class AnswerViewModel
    {
        public string Passport { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SeatReservationId { get; set; }
        public bool Answer { get; set; }
    }
}

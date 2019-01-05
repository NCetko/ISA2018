using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISA.Models.SeatViewModels
{
    public class EditViewModel
    {
        [Display(Name = "Airplane Name")]
        public string AirplaneName { get; set; }

        [Display(Name = "Segment Name")]
        public string SegmentName { get; set; }

        [Display(Name = "Seat Name")]
        public string SeatName { get; set; }

        [Required(ErrorMessage = "Required")]
        public int X { get; set; }

        [Required(ErrorMessage = "Required")]
        public int Y { get; set; }
    }
}

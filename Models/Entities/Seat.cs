using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ISA.Enums;

namespace ISA.Models.Entities
{
    public class Seat
    {
        public Seat() { }

        [Required(ErrorMessage = "Required")]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string SeatName { get; set; }
        public string SegmentName { get; set; }
        public string AirplaneName { get; set; }

        [ForeignKey("AirplaneName, SegmentName")]
        public Segment Segment { get; set; }

        [Required(ErrorMessage = "Required")]
        public int X { get; set; }

        [Required(ErrorMessage = "Required")]
        public int Y { get; set; }

    }

}

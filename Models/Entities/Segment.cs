using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ISA.Enums;

namespace ISA.Models.Entities
{
    public class Segment
    {
        public Segment() { }

        [Required(ErrorMessage = "Required")]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string SegmentName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Color { get; set; }

        public string AirplaneName { get; set; }

        [Required(ErrorMessage = "Required")]
        [ForeignKey("AirplaneName")]
        public Airplane Airplane { get; set; }

        public  ICollection<Seat> Seat { get; set; }
    }

}

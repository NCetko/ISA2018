using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.Entities
{
    public class Airplane
    {
        public Airplane() { }

        [Key]
        [StringLength(20)]
        [Display(Name = "Name")]
        public string AirplaneName { get; set; }

        public Airline Airline { get; set; }

        public ICollection<Segment> Segments { get; set; }
    }

}
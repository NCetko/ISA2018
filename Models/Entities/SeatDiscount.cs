using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ISA.Enums;

namespace ISA.Models.Entities
{
    public class SeatDiscount
    {
        public SeatDiscount() { }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiscountId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string SeatName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string SegmentName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string AirplaneName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string FlightName { get; set; }

        [Required(ErrorMessage = "Required")]
        [ForeignKey("SeatName, AirplaneName, SegmentName")]
        public Seat Seat { get; set; }

        [Required(ErrorMessage = "Required")]
        [ForeignKey("FlightName")]
        public Flight Flight { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue)]
        public int Price { get; set; }

        public string Passport { get; set; }

        public  Reservation Reservation { get; set; }

    }

}

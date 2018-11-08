using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ISA.Enums;

namespace ISA.Models.Entities
{
    public class SeatDiscount
    {
        public SeatDiscount() { }

        [Key]
        public int SeatDiscountId { get; set; }

        [Required(ErrorMessage = "Required")]
        public Seat Seat { get; set; }

        [Required(ErrorMessage = "Required")]
        public Flight Flight { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue)]
        public int Price { get; set; }

        public string Passport { get; set; }

        public  Reservation Reservation { get; set; }

    }

}

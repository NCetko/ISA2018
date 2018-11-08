using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ISA.Enums;

namespace ISA.Models.Entities
{
    public class RoomDiscount
    {
        public RoomDiscount() { }

        [Key]
        public int RoomDiscountId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1,int.MaxValue)]
        public int Price { get; set; }

        [Required(ErrorMessage = "Required")]
        [Column(TypeName = "date")]
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Required")]
        [Column(TypeName = "date")]
        [Display(Name = "End date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Required")]
        public Room Room { get; set; }

        public string Services { get; set; }

        public Reservation Reservation { get; set; }
    }

}

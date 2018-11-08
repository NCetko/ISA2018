using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ISA.Enums;

namespace ISA.Models.Entities
{
    public class Room
    {
        public Room() {
            Ratable = new Ratable();
        }

        [Required(ErrorMessage = "Required")]
        public int Floor { get; set; }

        [Display(Name = "Name")]
        public string RoomName { get; set; }

        [Range(1, int.MaxValue)]
        [Required(ErrorMessage = "Required")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Required")]
        public bool Balcony { get; set; }

        [Required(ErrorMessage = "Required")]
        public RoomType Type { get; set; }

        public string HotelName { get; set; }

        [ForeignKey("HotelName")]
        [Required(ErrorMessage = "Required")]
        public Hotel Hotel { get; set; }

        public Ratable Ratable { get; set; }

    }

}

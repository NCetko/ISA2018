using ISA.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.Entities
{
    public class HotelService
    {
        public HotelService() { }

        public HotelServiceType HotelServiceType { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue)]
        public int Price { get; set; }

        public string HotelName { get; set; }

        [ForeignKey("HotelName")]
        public Hotel Hotel { get; set; }
    }

}
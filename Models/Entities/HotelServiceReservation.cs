using ISA.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.Entities
{
    public class HotelServiceReservation
    {
        public HotelServiceReservation() { }

        [Key]
        public int HotelServiceReservationId { get; set; }

        public HotelService HotelService { get; set; }

        public Reservation Reservation { get; set; }
    } 

}
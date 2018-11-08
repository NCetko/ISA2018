using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.Entities
{
    public class Hotel
    {
        public Hotel() {
            Provider = new Provider();
            Ratable = new Ratable();
        }

        [Key]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string HotelName { get; set; }



        [Required(ErrorMessage = "Required")]
        public Destination Destination { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Required")]
        public Ratable Ratable { get; set; }

        [Required(ErrorMessage = "Required")]
        public Provider Provider { get; set; }

        public ICollection<Room> Room { get; set; }

        public byte[] Image { get; set; }
    }

}
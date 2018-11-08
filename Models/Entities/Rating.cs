using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISA.Models.Entities
{
    public class Rating
    {
        public Rating() { }

        [Key]
        public int RatingId { get; set; }

        [Required(ErrorMessage = "Required")]
        public Ratable Ratable { get; set; }

        [Required(ErrorMessage = "Required")]
        public ApplicationUser ApplicationUser { get; set; }

        [Required(ErrorMessage = "Required")]
        public Reservation Reservation { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Value { get; set; }
    }

}
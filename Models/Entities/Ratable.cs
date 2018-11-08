using ISA.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISA.Models.Entities
{
    public class Ratable
    {
        [Key]
        public int RatableId { get; set; }

        public ICollection<Rating> Ratings { get; set; }
    }
}

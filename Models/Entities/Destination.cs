using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.Entities
{
    public class Destination
    {
        public Destination() { }

        [Key]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string DestinationName { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(500)]
        public string Address { get; set; }

    }

}
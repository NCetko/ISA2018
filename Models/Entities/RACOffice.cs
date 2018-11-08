using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.Entities
{
    public class RACOffice
    {
        public RACOffice() { }

        [Required(ErrorMessage = "Required")]
        [StringLength(100)]
        public string Address { get; set; }


        public string RACName { get; set; }


        [ForeignKey("RACName")]
        public RAC RAC { get; set; }
    }

}
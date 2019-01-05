using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ISA.Models.SeatViewModels
{
    public class CreateViewModel
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string SeatName { get; set; }

        [Required(ErrorMessage = "Required")]
        public int X { get; set; }

        [Required(ErrorMessage = "Required")]
        public int Y { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISA.Models.AirplaneViewModels
{
    public class CreateViewModel
    {
        [StringLength(20)]
        [Display(Name = "Name")]
        public string AirplaneName { get; set; }

        [Display(Name = "Airline")]
        public string AirlineName { get; set; }
    }
}

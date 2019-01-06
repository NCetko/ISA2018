using ISA.Models.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.AirlineDestinationViewModels
{
    public class CreateViewModel
    {
        [StringLength(100)]
        [Display(Name = "Airline Name")]
        public string AirlineName { get; set; }


        [StringLength(100)]
        [Display(Name = "Destination Name")]
        public string DestinationName { get; set; }

        public CreateViewModel() { }


    }

}
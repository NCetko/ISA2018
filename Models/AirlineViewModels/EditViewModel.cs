using ISA.Models.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.AirlineViewModels
{
    public class EditViewModel
    {
        [StringLength(100)]
        [Display(Name = "Name")]
        public string AirlineName { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(500)]
        public string Description { get; set; }

        public IFormFile NewImage { get; set; }

        public byte[] Image { get; set; }

        public EditViewModel() { }
        public EditViewModel(Airline airline)
        {
            AirlineName = airline.AirlineName;
            Address = airline.Address;
            Description = airline.Description;
            Image = airline.Image;
        }


    }

}
using ISA.Models.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.AirlineViewModels
{
    public class CreateViewmModel
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

        public IFormFile Image { get; set; }

        public CreateViewmModel() { }


    }

}
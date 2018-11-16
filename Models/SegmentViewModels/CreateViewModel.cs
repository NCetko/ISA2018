using ISA.Models.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.SegmentViewModels
{
    public class CreateViewModel
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(20)]
        [Display(Name = "Name")]
        public string SegmentName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Color{ get; set; }
        public CreateViewModel() { }
    }
}
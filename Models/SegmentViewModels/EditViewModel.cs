using ISA.Models.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.SegmentViewModels
{
    public class EditViewModel
    {
        [Required(ErrorMessage = "Required")]
        public string Color { get; set; }
        public EditViewModel(Segment segment)
        {
            Color = segment.Color;
        }

        public EditViewModel() { }
    }
}
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ISA.Models.Entities
{
    public class Friendship
    {
        [Required(ErrorMessage = "Required")]
        public DateTime Created { get; set; }

        public string SenderId { get; set; }
        public string ReceiverId { get; set; }

        [ForeignKey("SenderId")]
        [Required(ErrorMessage = "Required")]
        public ApplicationUser Sender { get; set; }

        [ForeignKey("ReceiverId")]
        [Required(ErrorMessage = "Required")]
        public ApplicationUser Receiver { get; set; }

        public bool Approved { get; set; }

    }
}

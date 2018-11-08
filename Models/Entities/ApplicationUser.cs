using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ISA.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace ISA.Models.Entities
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser():base(){
            Points = 0;
        }

        [Required(ErrorMessage = "Required")]
        [StringLength(20)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(20)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(0, int.MaxValue)]
        public int Points { get; set; }

        public byte[] Image { get; set; }

        public  ICollection<Friendship> SentRequests { get; set; }
        public  ICollection<Friendship> ReceivedRequests { get; set; }
    }
}

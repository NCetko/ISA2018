using ISA.Models.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ISA.Models.AccountViewModels
{
    public class EditViewModel
    {
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
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Phone Number")]
        [StringLength(100)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100)]
        public string Address { get; set; }

        public IFormFile NewImage { get; set; }

        public byte[] Image { get; set; }

        public EditViewModel(){}

        public EditViewModel(ApplicationUser user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Address = user.Address;
            Image = user.Image;
        }

        //public byte[] Image { get; set; }
    }
}
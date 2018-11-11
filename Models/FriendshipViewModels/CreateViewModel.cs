using ISA.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISA.Models.FriendshipViewModels
{
    public class CreateViewModel
    {

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        public CreateViewModel(ApplicationUser user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            ReceiverId = user.Id;
        }

        public static List<CreateViewModel> FromList(List<ApplicationUser> users)
        {
            List<CreateViewModel> resultList = new List<CreateViewModel>();
            foreach(ApplicationUser user in users)
            {
                resultList.Add(new CreateViewModel(user));
            }
            return resultList;
        }
    }
}

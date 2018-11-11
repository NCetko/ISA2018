using ISA.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ISA.Models.FriendshipViewModels
{
    public class FriendsViewModel
    {
        [Required]
        public string ReceiverId { get; set; }
        [Required]
        public string SenderId { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public FriendsViewModel() { }

        public FriendsViewModel(Friendship friendship, string userId)
        {
            ReceiverId = friendship.ReceiverId;
            SenderId = friendship.SenderId;

            if (friendship.SenderId == userId)
            {
                FirstName = friendship.Receiver.FirstName;
                LastName = friendship.Receiver.LastName;
            }
            else
            {
                FirstName = friendship.Receiver.FirstName;
                LastName = friendship.Receiver.LastName;
            }
        }

        public static List<FriendsViewModel> Friends(List<Friendship> friendList, string userId)
        {
            List<FriendsViewModel> ResultList = new List<FriendsViewModel>();
            foreach (Friendship friend in friendList)
            {
                ResultList.Add(new FriendsViewModel(friend, userId));
            }

            return ResultList;
        }

        public static List<FriendsViewModel> Sent(List<Friendship> friendList)
        {
            List<FriendsViewModel> ResultList = new List<FriendsViewModel>();
            foreach (Friendship friend in friendList)
            {
                ResultList.Add(new FriendsViewModel
                {
                    FirstName = friend.Receiver.FirstName,
                    LastName = friend.Receiver.LastName,
                    SenderId = friend.SenderId,
                    ReceiverId = friend.ReceiverId
                });
            }

            return ResultList;
        }

        public static List<FriendsViewModel> Recieved(List<Friendship> friendList)
        {
            List<FriendsViewModel> ResultList = new List<FriendsViewModel>();
            foreach (Friendship friend in friendList)
            {
                ResultList.Add(new FriendsViewModel
                {
                    FirstName = friend.Sender.FirstName,
                    LastName = friend.Sender.LastName,
                    SenderId = friend.SenderId,
                    ReceiverId = friend.ReceiverId
                });
            }

            return ResultList;
        }
    }
}

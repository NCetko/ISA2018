using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISA.Data;
using ISA.Models.Entities;
using Microsoft.AspNetCore.Identity;
using ISA.Models.FriendshipViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ISA.Controllers
{

    [Authorize(Roles = "User")]
    public class FriendshipController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FriendshipController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Friendship/Friends
        public async Task<IActionResult> Friends()
        {
            var user = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            var applicationDbContext = _context.Friendships.Include(f => f.Receiver)
                .Include(f => f.Sender)
                .Where(f => f.Approved == true).Where(f => f.SenderId == user || f.ReceiverId == user);
            List<FriendsViewModel> friendList = FriendsViewModel.Friends(applicationDbContext.ToList(), user);
            return View(friendList);
        }

        public async Task<IActionResult> Sent()
        {
            var user = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            var applicationDbContext = _context.Friendships.Include(f => f.Receiver)
                .Include(f => f.Sender).Where(f => f.SenderId == user).Where(f => f.Approved == false);
            List<FriendsViewModel> friendList = FriendsViewModel.Sent(applicationDbContext.ToList());
            return View(friendList);
        }

        public async Task<IActionResult> Recieved()
        {
            var user = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            var applicationDbContext = _context.Friendships.Include(f => f.Receiver)
                .Include(f => f.Sender).Where(f => f.ReceiverId == user).Where(f => f.Approved == false);
            List<FriendsViewModel> friendList = FriendsViewModel.Recieved(applicationDbContext.ToList());
            return View(friendList);
        }

        // GET: Friendships/Create
        public async Task<IActionResult> Create()
        {
            var user = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            var users = (
                from u in _context.Users
                where !_context.Friendships.Any(
                    fs => (fs.SenderId == user || fs.ReceiverId == user)
                    && (fs.SenderId == u.Id || fs.ReceiverId == u.Id))
                select u).Where(u => u.Id != user);

            
            //ViewData["ReceiverId"] = new SelectList(users, "Id", "UserName");
            return View(CreateViewModel.FromList(users.ToList()));
        }

        // POST: Friendships/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/Friendship/Create/{reciever}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string reciever)
        {
            Friendship friendship = new Friendship
            {
                Receiver = _context.Users.Find(reciever),
                Sender = _context.Users.Find((await _userManager.GetUserAsync(HttpContext.User)).Id),
                Created = DateTime.Now,
                Approved = false
            };

            if (ModelState.IsValid)
            {
                _context.Add(friendship);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Sent));
            }
            ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "UserName");
            return View(friendship);

        }

        // POST: Friendships/Confirm/5
        [HttpPost("/Friendship/Confirm/{sender}/{reciever}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(string sender, string reciever)
        {
            var friendship = _context.Friendships.Find(sender, reciever);
            friendship.Approved = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Friends));
        }

        // GET: Friendships/Delete/5
        [HttpGet("/Friendship/Delete/{sender}/{reciever}")]
        public async Task<IActionResult> Delete(string sender, string reciever)
        {
            if (sender == null || reciever == null)
            {
                return NotFound();
            }

            var friendship = _context.Friendships.Find(sender, reciever);
            if (friendship == null)
            {
                return NotFound();
            }

            var user = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            return View(new FriendsViewModel(friendship, user));
        }

        // POST: Friendships/Delete/5
        [HttpPost("/Friendship/Delete/{sender}/{reciever}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string sender, string reciever)
        {
            var friendship = _context.Friendships.Find(sender, reciever);
            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Friends));
        }

        private bool FriendshipExists(string sender, string reciever)
        {
            return _context.Friendships.Any(e => e.SenderId == sender && e.ReceiverId == reciever);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Project_Hashtag.Data;
using Project_Hashtag.Models;


namespace Project_Hashtag.Pages
{
    public class FollowersModel : PageModel
    {
        private readonly AppDbContext database;
        private readonly AccessControl accessControl;


        public FollowersModel(AppDbContext database, AccessControl accessControl)
        {
            this.database = database;
            this.accessControl = accessControl;
        }

        public User User { get; set; } = default!;
        public List<Post> userPosts;
        public List<Comment> Comments;
        public List<Report> Reports = new List<Report>();
        public string FollowText;
        public int amountOfFollowers;
        public int amountFollowing;
        public string biography;
        public string avatar;

        public List<Follow> PeopleYouFollow;

        public List<int> Followers;

        public List<User> Users;

        public void OnGet()
        {
            int userId = accessControl.LoggedInAccountID;

            if (database.Users != null)
            {
                User = database.Users.Find(userId);
                userPosts = database.Posts.Where(x => x.UserID == userId).OrderByDescending(x => x.CreatedDate)
                    .ToList();
                Comments = database.Comments.OrderByDescending(x => x.CreatedDate).ToList();
                Users = database.Users.ToList();
                FollowText = Follow.GetFollowStatus(userId, accessControl.LoggedInAccountID, database);

                amountOfFollowers = database.Follows.Where(f => f.UserID == userId).Count();
                amountFollowing = database.Follows.Where(f => f.FollowingId == userId).Count();
                biography = User.Biography;
                avatar = User.Avatar;

                this.Followers = database.Follows.Where(f => User.ID == f.UserID).Where(f => User.ID != f.FollowingId).Select(x => x.FollowingId).ToList();

                Users = database.Users.Where(x => Followers.Contains(x.ID)).ToList();
            }
        }
    }
}

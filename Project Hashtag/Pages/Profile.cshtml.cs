using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_Hashtag.Data;
using Project_Hashtag.Models;

namespace Project_Hashtag.Pages
{
    public class ProfileModel : PageModel
    {
        readonly Project_Hashtag.Data.AppDbContext database;
        private AccessControl LoggedIn;


        public ProfileModel(Project_Hashtag.Data.AppDbContext context, AccessControl accessControl)
        {
            database = context;
            this.LoggedIn = accessControl;
        }


        public List<User> Users { get; set; } = new List<User>();
        public User User { get;set; } = default!;
        public List<Post> userPosts;
        public List<Comment> Comments;



        public IActionResult OnPostComment(int id, string content, int userId)
        {
            try
            {
                Comment.AddComment(LoggedIn.LoggedInAccountID, id, content, database);

                return RedirectToPage("Profile", new { id = userId });
            }
            catch
            {
                return RedirectToPage();
            }
        }

        public IActionResult OnPostLike(int id, int userId)
        {


            Post post = database.Posts.FirstOrDefault(x => x.ID == id);
            Like like = database.Likes.FirstOrDefault(x => x.PostID == post.ID && x.UserID == LoggedIn.LoggedInAccountID);

            if (like == null)
            {
                try
                {
                    like = new Like() { PostID = id, UserID = LoggedIn.LoggedInAccountID };
                    database.Likes.Add(like);
                    post.LikeCount += 1;
                    database.SaveChanges();

                    return RedirectToPage("Profile", new { id = userId });
                }
                catch
                {
                    return NotFound();
                }

            }
            else
            {
                try
                {
                    database.Likes.Remove(like);
                    post.LikeCount--;
                    database.SaveChanges();

                    return RedirectToPage("Profile", new { id = userId });
                }
                catch
                {
                    return NotFound();
                }
            }

        }
        public void OnGetAsync(int userId)
        {
            if (database.Users != null)
            {
                User =  database.Users.Single(u => u.ID == userId);
                userPosts = database.Posts.Where(x => x.UserID == userId).ToList();
                this.Comments = database.Comments.ToList();
                this.Users = database.Users.ToList();
            }
        }
    }
}

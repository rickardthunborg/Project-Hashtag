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
        private readonly Project_Hashtag.Data.AppDbContext database;
        private AccessControl LoggedIn;


        public ProfileModel(Project_Hashtag.Data.AppDbContext context, AccessControl accessControl)
        {
            database = context;
            this.LoggedIn = accessControl;
        }

        public User User { get;set; } = default!;
        public List<Post> userPosts;
        public List<Comment> Comments;



        public IActionResult OnPostComment(int id, string content)
        {


            Comment comment = new Comment() { Text = content, PostID = id, UserID = LoggedIn.LoggedInAccountID };
            database.Comments.Add(comment);
            database.SaveChanges();

            return RedirectToPage("./Profile" + User.ID);


        }

        public IActionResult OnPostLike(int id)
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

                    return RedirectToPage(new { id = User.ID});
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

                    return RedirectToPage(new { id = User.ID });
                }
                catch
                {
                    return NotFound();
                }
            }

        }
        public void OnGetAsync(int id)
        {
            if (database.Users != null)
            {
                User =  database.Users.Single(u => u.ID == id);
                userPosts = database.Posts.Where(x => x.UserID == id).ToList();
                this.Comments = database.Comments.ToList();
            }
        }
    }
}

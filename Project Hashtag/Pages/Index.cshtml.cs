using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using Project_Hashtag.Data;
using Project_Hashtag.Models;
using System.ComponentModel.Design;

namespace Project_Hashtag.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext database;
        private readonly AccessControl LoggedIn;
        public List<Post> Posts = new List<Post>();
        public List<User> Users;
        public List<Comment> Comments;
        public List<Tag> Tags = new List<Tag>();
        public List<Report> Reports = new List<Report>();
        public int amountOfReporters;


        public IndexModel(AppDbContext database, AccessControl accessControl)
        {
            this.database = database;

            this.LoggedIn = accessControl;

        }

        public void OnGet()
        {
            this.Posts = database.Posts.OrderByDescending(x => x.CreatedDate).ToList();
            this.Users = database.Users.ToList();
            this.Tags = database.Tags.ToList();
            this.Comments = database.Comments.ToList();
            this.Reports = database.Reports.ToList();
            var peopleYouFollow = database.Follows.Where(f => f.UserID == LoggedIn.LoggedInAccountID).ToList();

            this.amountOfReporters = database.Reports.Where(r => peopleYouFollow.Select(p => p.FollowingId).Contains(r.UserID)).Count();






        }


        public IActionResult OnPostComment(int id, string content)
        {
            try
            {
                Comment.AddComment(LoggedIn.LoggedInAccountID, id, content, database);

                return RedirectToPage("/index");
            }
            catch
            {
                return RedirectToPage();
            }
        }

        public IActionResult OnPostReport(int id)
        {
            Post post = database.Posts.FirstOrDefault(x => x.ID == id);
            Report report = database.Reports.FirstOrDefault(x => x.PostID == post.ID && x.UserID == LoggedIn.LoggedInAccountID);

            if (report == null)
            {
                try
                {
                    report = new Report() { PostID = id, UserID = LoggedIn.LoggedInAccountID };
                    database.Reports.Add(report);
                    database.SaveChanges();

                    return RedirectToPage();
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
                    database.Reports.Remove(report);
                    database.SaveChanges();

                    return RedirectToPage();
                }
                catch
                {
                    return NotFound();
                }
            }

        }

        public IActionResult OnPostLike(int id, string returnURL)
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

                    return RedirectToPage();
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

                    return RedirectToPage();
                }
                catch
                {
                    return NotFound();
                }
            }

        }
        
        public async Task<IActionResult> OnPost(int tag, string desc)
        {
            try
            {
                var post = new Post { UserID = LoggedIn.LoggedInAccountID, TagID = tag, Description = desc };
                database.Posts.Add(post);
                await database.SaveChangesAsync();

                return RedirectToPage("/index");
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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


        public IndexModel(AppDbContext database, AccessControl accessControl)
        {
            this.database = database;

            this.LoggedIn = accessControl;

        }

        public void OnGet()
        {
            this.Posts = database.Posts.OrderBy(p => p.CreatedDate).ToList();
            this.Users = database.Users.ToList();
            this.Tags = database.Tags.ToList();
            this.Comments = database.Comments.ToList();

        }


        public IActionResult OnPostComment(int id, string content)
        {
           

            Comment comment = new Comment() { Text = content, PostID = id, UserID = LoggedIn.LoggedInAccountID };
            database.Comments.Add(comment);
            database.SaveChanges();

            return RedirectToPage("/index");


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

                    return RedirectToPage("/index");
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

                    return RedirectToPage("/index");
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Storage;
using Project_Hashtag.Data;
using Project_Hashtag.Models;
using System.ComponentModel.Design;

namespace Project_Hashtag.Pages
{
    public class PostModel : PageModel
    {

		readonly Project_Hashtag.Data.AppDbContext database;
		public AccessControl LoggedIn;
		public Post Post { get; set; }
		public List<User> Users { get; set; } = new List<User>();
		public User User;
		public List<Comment> Comments;
        public List<Report> Reports = new List<Report>();




        public PostModel(Project_Hashtag.Data.AppDbContext context, AccessControl accessControl)
		{
			database = context;
			this.LoggedIn = accessControl;
            Users = database.Users.ToList();
		}

        public IActionResult OnGet(int postID)
        {
			Post = database.Posts.Find(postID);
			if(Post == null)
			{
				return NotFound();
			}
			User = database.Users.Single(u => u.ID == Post.UserID);
			Comments = database.Comments.Where(c => c.PostID == Post.ID).OrderByDescending(c => c.CreatedDate).ToList();
			Reports = database.Reports.Where(r => r.PostID == Post.ID).ToList();

			return Page();
        }

        public IActionResult OnPostComment(int postID, string content)
        {
            try
            {
                Comment.AddComment(LoggedIn.LoggedInAccountID, postID, content, database);
                return RedirectToPage("Post", new { postID = postID });
            }
            catch
            {
                return NotFound();
            }
        }

		public IActionResult OnPostDeleteComment(int commentId)
		{
			try
			{
				Comment comment = database.Comments.Find(commentId);
				if (comment.UserID != LoggedIn.LoggedInAccountID)
				{
					return Forbid();
				}
				Comment.DeleteComment(comment, database);

				return RedirectToPage();
			}
			catch
			{
				return RedirectToPage();
			}
		}

        public IActionResult OnPostLike(int postID)
        {

            Post post = database.Posts.FirstOrDefault(x => x.ID == postID);
            Like like = database.Likes.FirstOrDefault(x => x.PostID == post.ID && x.UserID == LoggedIn.LoggedInAccountID);

            if (like == null)
            {
                try
                {
                    like = new Like() { PostID = postID, UserID = LoggedIn.LoggedInAccountID };
                    database.Likes.Add(like);
                    post.LikeCount += 1;
                    database.SaveChanges();

                    return RedirectToPage("Post", new { id = postID });
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

                    return RedirectToPage("Post", new { id = postID });
                }
                catch
                {
                    return NotFound();
                }
            }

        }
    }
}

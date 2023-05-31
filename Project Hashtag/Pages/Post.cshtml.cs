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

		public Project_Hashtag.Data.AppDbContext database;
		public AccessControl LoggedIn;
		public Post Post { get; set; }
		public List<User> Users { get; set; } = new List<User>();
		public User User;
		public List<Comment> Comments;
        public List<Report> Reports = new List<Report>();
        public List<int> FollowingIds;
        private readonly string baseDirectoryPath;






        public PostModel(Project_Hashtag.Data.AppDbContext context, AccessControl accessControl)
		{
			database = context;
			this.LoggedIn = accessControl;
            Users = database.Users.ToList();

            string currentDirectory = Directory.GetCurrentDirectory();
            string parentDirectory = Directory.GetParent(currentDirectory)?.FullName;
            string grandparentDirectory = Directory.GetParent(parentDirectory)?.FullName;
            baseDirectoryPath = Path.Combine(grandparentDirectory, "UploadedFiles");
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
            FollowingIds = database.Follows
                     .Where(f => f.FollowingId == LoggedIn.LoggedInAccountID)
                     .Select(f => f.UserID)
                     .ToList();

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

        public IActionResult OnPostDeleteComment(int id)
        {
            try
            {
                Comment? comment = database.Comments.Find(id);
                if (comment.UserID != LoggedIn.LoggedInAccountID)
                {
                    return Forbid();
                }

                database.Comments.Remove(comment);
                database.SaveChanges();

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

                    return RedirectToPage("Post", new { postID = postID });
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

                    return RedirectToPage("Post", new { postID = postID });
                }
                catch
                {
                    return NotFound();
                }
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

        public async Task<IActionResult> OnPostDelete(int postID, int userID)
        {

            try
            {
                Post post = database.Posts.Find(postID);
                if (post.UserID != userID)
                {
                    return Forbid();
                }

				if (post.PictureUrl != null)
				{
					string relativePath = post.PictureUrl.Replace("/uploads/", string.Empty);
					string imagePath = baseDirectoryPath + "\\" + relativePath;

					if (System.IO.File.Exists(imagePath))
					{
						// Delete the file
						System.IO.File.Delete(imagePath);
						Console.WriteLine("Image deleted successfully.");
					}
					else
					{
						Console.WriteLine("Image file not found.");
					}
				}

				var likesToRemove = database.Likes.Where(l => l.PostID == postID);

				if (likesToRemove.Count() > 0)
				{
					database.Likes.RemoveRange(likesToRemove);
				}

				database.Posts.Remove(post);
                await database.SaveChangesAsync();

				return RedirectToPage("index");
			}
            catch
            {
                return NotFound();
            }
        }
    }
}

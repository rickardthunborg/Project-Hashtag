using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using Project_Hashtag.Data;
using Project_Hashtag.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

namespace Project_Hashtag.Pages
{
    public class IndexModel : PageModel
    {
        public readonly AppDbContext database;
        public readonly AccessControl LoggedIn;
        private readonly FileRepository uploads;


        public List<Post> FollowingPosts = new List<Post>();
        public List<Post> OtherPosts = new List<Post>();

        public Post Post;
        public List<User> Users;
        public List<Comment> Comments;
        public List<Report> Reports = new List<Report>();
        public List<int> FollowingIds;
        public string search;


        public IndexModel(AppDbContext database, AccessControl accessControl, FileRepository uploads)
        {
            this.database = database;

            this.LoggedIn = accessControl;

            this.uploads = uploads;

        }

        public IActionResult OnGet()
        {
            string searchQuery = Request.Query["search"];

            if (!string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToPage("/search", new { search = searchQuery });
            }

            FollowingIds = database.Follows
                .Where(f => f.FollowingId == LoggedIn.LoggedInAccountID)
                .Select(f => f.UserID)
                .ToList();

            this.FollowingPosts = database.Posts
                 .Where(p => p.UserID != LoggedIn.LoggedInAccountID) 
                 .Where(p => FollowingIds.Contains(p.UserID)) 
                 .Where(p => p.CreatedDate >= DateTime.Now.AddDays(-1))
                 .OrderByDescending(x => x.CreatedDate)
                 .ToList();

            OtherPosts = database.Posts
                .Where(p => p.UserID != LoggedIn.LoggedInAccountID) 
                .OrderByDescending(x => x.CreatedDate)
                .ToList()
                .Except(this.FollowingPosts)
                .ToList();

            this.Users = database.Users.ToList();
            this.Comments = database.Comments.ToList();
            this.Reports = database.Reports.ToList();
            return Page();

        }

        public IActionResult OnPostDeleteComment(int id)
        {
            try
            {
                Comment comment = database.Comments.Single(c => c.PostID == id);
                if (comment.UserID != LoggedIn.LoggedInAccountID)
                {
                    return Forbid();
                }
                Comment.DeleteComment(comment, database);

                string returnUrl = Url.Page("/index") + "#" + id;
                return Redirect(returnUrl);
            }
            catch
            {
                return RedirectToPage();
            }
        }

        public IActionResult OnPostComment(int id, string content)
        {
            try
            {
                Comment.AddComment(LoggedIn.LoggedInAccountID, id, content, database);

                string returnUrl = Url.Page("/index") + "#" + id;
                return Redirect(returnUrl);
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

                    string returnUrl = Url.Page("/index") + "#" + id;
                    return Redirect(returnUrl);
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

                    string returnUrl = Url.Page("/index") + "#" + id;
                    return Redirect(returnUrl);
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

                    string returnUrl = Url.Page("/index") + "#" + id;
                    return Redirect(returnUrl);
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

                    string returnUrl = Url.Page("/index") + "#" + id;
                    return Redirect(returnUrl);
                }
                catch
                {
                    return NotFound();
                }
            }

        }

        [BindProperty]
        [Required(ErrorMessage = "Description is required. Please provide a description for the post.")]
        [MaxLength(500, ErrorMessage = "Description is to long, maximum is 500 characters")]
        public string desc { get; set; }

        [BindProperty]
        [MaxLength(20, ErrorMessage = "Tag cannot be over 20 characters")]
        public string? Tag { get; set; }


        public async Task<IActionResult> OnPost(IFormFile? photo)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                Post? post = new Post { UserID = LoggedIn.LoggedInAccountID, Tag = Post.FormatTag(this.Tag), Description = desc };

                if (photo == null)
                {
                    database.Posts.Add(post);
                    await database.SaveChangesAsync();
                }
                else
                {
                    if (photo.ContentType != "image/jpeg" && photo.ContentType != "image/png")
                    {
                        return BadRequest("Invalid file type. Please upload a JPEG or PNG image.");
                    }

                    string path = Path.Combine(
                    Guid.NewGuid().ToString() + "-" + photo.FileName);
                    await uploads.SaveFileAsync(photo, path);
                    post = new Post { UserID = LoggedIn.LoggedInAccountID, Tag = Post.FormatTag(this.Tag), Description = desc, PictureUrl = "/uploads/" + path };
                    database.Posts.Add(post);

                    await database.SaveChangesAsync();
                }
                return RedirectToPage("Post", new { postID = post.ID});

            }
            catch
            {
                return NotFound();
            }
        }
    }
}
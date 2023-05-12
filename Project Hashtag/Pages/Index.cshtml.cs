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


        public List<Post> Posts = new List<Post>();
        public List<User> Users;
        public List<Comment> Comments;
        public List<Report> Reports = new List<Report>();
        public List<Follow> PeopleYouFollow;
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

            this.Posts = database.Posts.OrderByDescending(x => x.CreatedDate).ToList();
            this.Users = database.Users.ToList();
            this.Comments = database.Comments.ToList();
            this.Reports = database.Reports.ToList();
            PeopleYouFollow = database.Follows.Where(f => f.FollowingId == LoggedIn.LoggedInAccountID).ToList();
            return Page();

        }

        public IActionResult OnPostDeleteComment(int id)
        {
            try
            {
                Comment comment = database.Comments.Single(c => c.PostID == id);
                if(comment.UserID != LoggedIn.LoggedInAccountID)
                {
                    return Forbid();
                }
                Comment.DeleteComment(comment, database);

                return RedirectToPage("/index");
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

        [BindProperty]
        [Required(ErrorMessage = "Description is required. Please provide a description for the post.")]
        [MaxLength(500, ErrorMessage = "Description is to long, maximum is 500 characters")]
        public string desc { get; set; }


        public async Task<IActionResult> OnPost(string tag,  IFormFile? photo)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if (photo == null)
                {
                    var post = new Post { UserID = LoggedIn.LoggedInAccountID, Tag = tag, Description = desc };
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
                    var post = new Post { UserID = LoggedIn.LoggedInAccountID, Tag = tag, Description = desc , PictureUrl =  "/uploads/" + path};
                    database.Posts.Add(post);

                    await database.SaveChangesAsync();
                }

                return RedirectToPage("/index");
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
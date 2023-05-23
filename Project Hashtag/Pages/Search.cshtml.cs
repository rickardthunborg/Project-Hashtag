using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_Hashtag.Data;
using Project_Hashtag.Models;


namespace Project_Hashtag.Pages
{
    public class SearchModel : PageModel
    {
        public readonly AppDbContext database;
        public readonly AccessControl LoggedIn;


        public List<Post> Posts = new List<Post>();
        public List<User> Users = new List<User>();
        public List<Comment> Comments;
        public List<Post> QueriedPosts = new List<Post>();
        public List<User> QueriedUsers = new List<User>();
        public List<Report> Reports = new List<Report>();
        public List<Follow> PeopleYouFollow;
        public List<int> FollowingIds;

        public string Search;
        public bool OnlyTags;

        public bool orderByLikes = true;


        public SearchModel(AppDbContext database, AccessControl accessControl)
        {
            this.database = database;
            this.LoggedIn = accessControl;
            this.Posts = database.Posts.ToList();
            this.Users = database.Users.ToList();
            this.Comments = database.Comments.ToList();
            this.Reports = database.Reports.ToList();
            this.PeopleYouFollow = database.Follows.Where(f => f.FollowingId == LoggedIn.LoggedInAccountID).ToList();



        }

        public IActionResult OnGet(string? search, string? sort)
        {
            this.orderByLikes = sort == "pop";
            this.Search = search;

            search = search.ToLower();

            if (string.IsNullOrEmpty(search))
            {
                return Page();
                
            }

            this.QueriedPosts = database.Posts
                .Where(p => p.Description.ToLower().Contains(search) || p.Tag!.Contains(search))
                .OrderByDescending(p => p.CreatedDate)
                .ToList();

            if (orderByLikes)
            {
                QueriedPosts = QueriedPosts
                    .OrderByDescending(p => p.LikeCount)
                    .ToList();
            }

            this.QueriedUsers = database.Users.Where(u => u.Name.ToLower().Contains(search)).ToList();
            FollowingIds = database.Follows
                     .Where(f => f.FollowingId == LoggedIn.LoggedInAccountID)
                     .Select(f => f.UserID)
                     .ToList();

            return Page();

        }

        public IActionResult OnPostComment(int postID, string content, string search)
        {
            try
            {
                Comment.AddComment(LoggedIn.LoggedInAccountID, postID, content, database);


                return RedirectToPage("/Search", new { search });

            }
            catch
            {
                return RedirectToPage();
            }
        }

        public IActionResult OnPostDeleteComment(int commentId, string search)
        {
            try
            {
                Comment comment = database.Comments.Find(commentId);
                if (comment.UserID != LoggedIn.LoggedInAccountID)
                {
                    return Forbid();
                }
                Comment.DeleteComment(comment, database);

                return RedirectToPage("/Search", new { search });

            }
            catch
            {
                return RedirectToPage();
            }
        }

        public IActionResult OnPostLike(int id, string returnURL, string search)
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

                    return RedirectToPage("/Search", new { search });
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

                    return RedirectToPage("/Search", new { search });
                }
                catch
                {
                    return NotFound();
                }
            }

        }

        public IActionResult OnPostReport(int id, string search)
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

                    return RedirectToPage("/Search", new { search });
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

                    return RedirectToPage("/Search", new { search });
                }
                catch
                {
                    return NotFound();
                }
            }

        }
    }
}

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
        public string search;



        public SearchModel(AppDbContext database, AccessControl accessControl)
        {
            this.database = database;
            this.LoggedIn = accessControl;
            this.Posts = database.Posts.ToList();
            this.Users = database.Users.ToList();
            this.Comments = database.Comments.ToList();

        }

        public IActionResult OnGet(string search)
        {
            search = search?.ToLower();

            if (string.IsNullOrEmpty(search))
            {
                return Page();
                
            }

            this.QueriedPosts = database.Posts.Where(p => p.Description.ToLower().Contains(search)).ToList();
            this.QueriedUsers = database.Users.Where(u => u.Name.ToLower().Contains(search)).ToList();
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
    }
}

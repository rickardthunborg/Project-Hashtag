using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_Hashtag.Data;
using Project_Hashtag.Models;

namespace Project_Hashtag.Pages
{
    public class ProfileSettingsModel : PageModel
    {
        readonly Project_Hashtag.Data.AppDbContext database;
        public AccessControl LoggedIn;


        public ProfileSettingsModel(Project_Hashtag.Data.AppDbContext context, AccessControl accessControl)
        {
            database = context;
            this.LoggedIn = accessControl;
        }


        public List<User> Users { get; set; } = new List<User>();
        public User User { get; set; } = default!;
        public List<Post> userPosts;
        public List<Comment> Comments;
        public string FollowText;
        public int amountOfFollowers;
        public int amountFollowing;
        public string search;
        public string biography;
        public string avatar;


        public IActionResult OnGetAsync(int userId)
        {
            if (LoggedIn.LoggedInAccountID != userId)
            {
                return Forbid();
            }

            if (database.Users != null)
            {
                try
                {
                    User = database.Users.Find(userId);
                    this.biography = User.Biography;
                    this.avatar = User.Avatar;
                    return Page();
                }
                catch
                {
                    this.biography = "";
                }
            }

            return NotFound();
        }
    }
}

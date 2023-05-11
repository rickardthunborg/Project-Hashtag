using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Storage;
using Project_Hashtag.Data;
using Project_Hashtag.Models;

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



		public PostModel(Project_Hashtag.Data.AppDbContext context, AccessControl accessControl)
		{
			database = context;
			this.LoggedIn = accessControl;
		}

        public IActionResult OnGet(int postID)
        {
			Post = database.Posts.Find(postID);
			if(Post == null)
			{
				return NotFound();
			}
			User = database.Users.Single(u => u.ID == Post.UserID);

			return Page();
        }
    }
}

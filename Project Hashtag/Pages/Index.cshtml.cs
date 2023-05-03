using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_Hashtag.Data;
using Project_Hashtag.Models;

namespace Project_Hashtag.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext database;
        private readonly AccessControl LoggedIn;
        public List<Post> Posts;
        public List<User> Users;
        public List<Tag> Tags;


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

        }


        [HttpPost]
        public async Task<IActionResult> CreatePost(int tag, string desc)
        {
            try
            {
                var post = new Post { UserID = LoggedIn.LoggedInAccountID, TagID = tag, Description = desc };
                database.Posts.Add(post);
                await database.SaveChangesAsync();

                return RedirectToPage();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
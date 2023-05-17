using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Hashtag.Data;
using Project_Hashtag.Models;
using static System.Net.WebRequestMethods;

namespace Project_Hashtag.Controllers
{
    [Route("/api")]
    [ApiController]

    [AllowAnonymous]
    public class APIController : ControllerBase
    {
        private readonly AppDbContext database;

        public APIController(AppDbContext database)
        {
            this.database = database;
        }

        [HttpGet("/posts")]
        public ActionResult<IEnumerable<Post>> GetPost(string tag)
        {
            var posts = database.Posts.Where(p => p.Tag.ToLower() == tag.ToLower()).ToList();

            Post? post = posts.OrderByDescending(p => p.LikeCount).FirstOrDefault();

            if (post == null)
            {
                return NotFound(new { message = "No relatable posts were found." });
            }

            var returnURL = "https://facegram.azurewebsites.net/Post/" + post.ID;

            var imageURL = "https://facegram.azurewebsites.net" + post.PictureUrl; 

            return Ok( new { postContent = post.Description, imageURL, postURL = returnURL, poster = post.User.Name});
        }
    }
}

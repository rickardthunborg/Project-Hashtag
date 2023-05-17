using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Project_Hashtag.Data;
using Project_Hashtag.Models;

namespace Project_Hashtag.Pages
{
    public class ProfileModel : PageModel
    {
        public Project_Hashtag.Data.AppDbContext database;
        public AccessControl LoggedIn;
        public List<int> FollowingIds;


        public ProfileModel(Project_Hashtag.Data.AppDbContext context, AccessControl accessControl)
        {
            database = context;
            this.LoggedIn = accessControl;

            

        }


        public List<User> Users { get; set; } = new List<User>();
        public User User { get;set; } = default!;
        public List<Post> userPosts;
        public List<Comment> Comments;
        public List<Report> Reports = new List<Report>();
        public string FollowText;
        public int amountOfFollowers;
        public int amountFollowing;
        public string search;
        public string biography;
        public string avatar;
        public List<Follow> PeopleYouFollow;




        public IActionResult OnPostDelete(int id, int userId)
        {

            try
            {
                Post post = database.Posts.Find(id);
                if(post.UserID != userId)
                {
                    return Forbid();
                }
                Post.DeletePost(post, database);

                return RedirectToPage("Profile", new { id = userId });
            }
            catch
            {
                return NotFound();
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


        public IActionResult OnPostComment(int id, string content, int userId)
        {
            try
            {
                Comment.AddComment(LoggedIn.LoggedInAccountID, id, content, database);

                return RedirectToPage("Profile", new { id = userId });
            }
            catch
            {
                return RedirectToPage();
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

        public IActionResult OnPostLike(int id, int userId)
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

                    return RedirectToPage("Profile", new { userId = userId });
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

                    return RedirectToPage("Profile", new { userId = userId });
                }
                catch
                {
                    return NotFound();
                }
            }

        }
        public IActionResult OnPostFollow(int userId)
        {
            string followStatus = Follow.GetFollowStatus(userId, LoggedIn.LoggedInAccountID, database);

            if (followStatus == "Follow" || followStatus == "Follow back")
            {
                database.Follows.Add(new Follow { FollowingId = LoggedIn.LoggedInAccountID, UserID = userId });
                database.SaveChanges();

                return RedirectToPage("Profile", new { id = userId });
            }
            else if (followStatus == "Unfollow")
            {
                Follow f = database.Follows.Single(x => x.FollowingId == LoggedIn.LoggedInAccountID && x.UserID == userId);
                database.Follows.Remove(f);
                database.SaveChanges();

                return RedirectToPage("Profile", new { id = userId });
            }

            


            return RedirectToPage("Profile", new { id = userId });
        }


        public void OnGetAsync(int userId)
        {
            if (database.Users != null)
            {
                User =  database.Users.Find(userId);
                userPosts = database.Posts.Where(x => x.UserID == userId).OrderByDescending(x => x.CreatedDate).ToList();
                this.Comments = database.Comments.OrderByDescending(x => x.CreatedDate).ToList();
                this.Users = database.Users.ToList();
                this.FollowText = Follow.GetFollowStatus(userId, LoggedIn.LoggedInAccountID, database);
                this.amountOfFollowers = database.Follows.Where(f => f.UserID == User.ID).Count();
                this.amountFollowing = database.Follows.Where(f => f.FollowingId == User.ID).Count();
                this.biography = User.Biography;
                this.avatar = User.Avatar;

                FollowingIds = database.Follows
                     .Where(f => f.FollowingId == LoggedIn.LoggedInAccountID)
                     .Select(f => f.UserID)
                     .ToList();

                this.Reports = database.Reports.ToList();
            }
        }
    }
}

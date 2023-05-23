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

namespace Project_Hashtag.Pages;

public class FollowingModel : PageModel
{
    private readonly AppDbContext database;
    private readonly AccessControl accessControl;

    public FollowingModel(AppDbContext database, AccessControl accessControl)
    {
        this.database = database;
        this.accessControl = accessControl;
    }


    public User User { get; set; } = default!;
    public List<Post> userPosts;
    public List<Comment> Comments;
    public List<Report> Reports = new List<Report>();
    public string FollowText;
    public int amountOfFollowers;
    public int amountFollowing;
    public string biography;
    public string avatar;

    
    public List<int> Following;

    public List<User> Users;

    public void OnGet()
    {
        int userId = accessControl.LoggedInAccountID;

        if (database.Users != null)
        {
            User = database.Users.Find(userId);
            userPosts = database.Posts.Where(x => x.UserID == userId).OrderByDescending(x => x.CreatedDate)
                .ToList();
            Comments = database.Comments.OrderByDescending(x => x.CreatedDate).ToList();
            Users = database.Users.ToList();
            FollowText = Follow.GetFollowStatus(userId, accessControl.LoggedInAccountID, database);
            
            //this counts
            amountFollowing = database.Follows.Where(f => f.FollowingId == userId).Count();

            
            this.Following = database.Follows.Where(f => f.FollowingId == userId).Select(x => x.UserID).ToList();

            
            Users = database.Users.Where(x => Following.Contains(x.ID)).ToList();

        }
    }
}
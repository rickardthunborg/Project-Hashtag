using System.ComponentModel.DataAnnotations.Schema;
using Project_Hashtag.Data;

namespace Project_Hashtag.Models
{
    public class Comment
    {
        public int Id { get; set; } 

        public string Text { get; set; }

        public int PostID { get; set; }

        public int UserID { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public User User { get; set; }
        public Post Post { get; set; }


        public static void AddComment(int userId, int postId ,string text, AppDbContext database)
        {
            Comment comment = new Comment() { Text = text, PostID = postId, UserID = userId };
            database.Comments.Add(comment);
            database.SaveChanges();
        }

        public static void DeleteComment(Comment comment, AppDbContext database)
        {
            database.Comments.Remove(comment);
            database.SaveChanges();
        }
        public string TimeSinceComment()
        {
            var timeSince = DateTime.Now.Subtract(CreatedDate);

            if (timeSince.Days > 0)
            {
                return $"{timeSince.Days} d";
            }
            else if (timeSince.Hours > 0)
            {
                return $"{timeSince.Hours} h";
            }
            else if (timeSince.Minutes > 0)
            {
                return $"{timeSince.Minutes} m";
            }
            else
            {
                return $"{timeSince.Seconds} s";
            }
        }
    }
}

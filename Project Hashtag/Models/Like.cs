using Project_Hashtag.Data;

namespace Project_Hashtag.Models
{
    public class Like
    {
        public int Id { get; set; }

        public int PostID { get; set; }

        public int UserID { get; set; }

        public User User { get; set; }

        public Post Post { get; set; }

        public DateTime DateLiked { get; set; } = DateTime.Now;

        public string TimeSinceLike()
        {
            var timeSince = DateTime.Now.Subtract(DateLiked);

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

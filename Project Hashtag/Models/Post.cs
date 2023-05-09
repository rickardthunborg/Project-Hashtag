namespace Project_Hashtag.Models
{
    public class Post
    {
        public int ID { get; set; }

        public string Description { get; set; }

        public string? PictureUrl { get; set; }

        public int LikeCount { get; set; } = 0;

        public int UserID { get; set; }

        public int? TagID { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Post() { }

        public User User { get; set; }

        public Tag? Tag { get; set; }

        public List<Comment> Comments { get; set; } =  new List<Comment>();

        public List<Like> Likes { get; set; }

        public List<Report> Reports { get; set; }

        public string TimeSincePost()
        {
            var timeSince = DateTime.Now.Subtract(CreatedDate);

            if (timeSince.Days > 0)
            {
                return $"{timeSince.Days} d ago";
            }
            else if (timeSince.Hours > 0) 
            { 
                return $"{timeSince.Hours} h ago";
            }
            else if (timeSince.Minutes > 0)
            {
                return $"{timeSince.Minutes} m ago";
            }
            else
            {
                return $"{timeSince.Seconds} s ago";
            }
        }
    }
}

namespace Project_Hashtag.Models
{
    public class Post
    {
        public int ID { get; set; }

        public string Description { get; set; }

        public string? PictureUrl { get; set; }

        public int Likes { get; set; } = 0;

        public int UserID { get; set; }

        public int? TagID { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Post() { }

        public User User { get; set; }

        public Tag? Tag { get; set; }

        public List<Comment> Comments { get; set; } =  new List<Comment>();

    }
}

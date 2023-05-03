namespace Project_Hashtag.Models
{
    public class Like
    {
        public int Id { get; set; }

        public int PostID { get; set; }

        public int UserID { get; set; }

        public User User { get; set; }

        public Post Post { get; set; }
    }
}

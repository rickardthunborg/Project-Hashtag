namespace Project_Hashtag.Models
{
    public class Report
    {
        public int ID { get; set; }

        public int PostID { get; set; }

        public int UserID { get; set; }

        public User User { get; set; }

        public Post Post { get; set; }


    }
}

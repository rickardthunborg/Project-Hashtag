using System.ComponentModel.DataAnnotations.Schema;

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
    }
}

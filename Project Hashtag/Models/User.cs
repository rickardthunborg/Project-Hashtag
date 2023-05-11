using NuGet.Configuration;

namespace Project_Hashtag.Models
{
    public class User
    {
        public int ID { get; set; }
        public string OpenIDIssuer { get; set; }
        public string OpenIDSubject { get; set; }
        public string Name { get; set; }
        public string? Avatar { get; set; }
        public string? Description { get; set; }

        public List<Like> Likes { get; set; }
        public List<Follow> Followers { get; set; }

        public List<Report> Reports { get; set; }
        
    }
}

using Project_Hashtag.Data;

namespace Project_Hashtag.Models
{
    public class Follow
    {
        public int UserID { get; set; }
        public User Follower { get; set; }

        public int FollowingId { get; set; }
        public User Following { get; set; }    

        public static string GetFollowStatus(int userId, int following, AppDbContext database)
        {
            bool follows = database.Follows.Any(x => x.UserID == userId && x.FollowingId == following);
            bool followsBack = database.Follows.Any(x => x.UserID == following && x.FollowingId == userId);

            if (!follows)
            {
                return "Follow";
            }
            else if (!follows && followsBack)
            {
                return "Follow back";
            }
            else if (follows)
            {
                return "Unfollow";
            }

            return "Error";

        }

    }
}
 
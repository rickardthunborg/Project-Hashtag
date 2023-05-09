using Project_Hashtag.Data;

namespace Project_Hashtag.Models
{
    public class Follow
    {
        public int FollowerId { get; set; }
        public User Follower { get; set; }

        public int FollowingId { get; set; }
        public User Following { get; set; }    

        public static string GetFollowStatus(int followed, int following, AppDbContext database)
        {
            bool follows = database.Follows.Any(x => x.FollowingId == followed && x.FollowingId == following);
            bool followsBack = database.Follows.Any(x => x.FollowingId == following && x.FollowingId == followed);

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
 
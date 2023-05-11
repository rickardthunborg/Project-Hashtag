using Project_Hashtag.Models;
using System.Collections.Generic;

namespace Project_Hashtag.Data
{
    public class SampleData
    {
        public static void Create(AppDbContext database)
        {
            // If there are no fake accounts, add some.
            string fakeIssuer = "https://example.com";
            if (!database.Users.Any(a => a.OpenIDIssuer == fakeIssuer))
            {
                database.Users.Add(new User
                {
                    OpenIDIssuer = fakeIssuer,
                    OpenIDSubject = "1111111111",
                    Name = "Brad",
                    Biography = "I'm a pizza-loving astronaut who dreams of playing guitar on Mars."
                });
                database.Users.Add(new User
                {
                    OpenIDIssuer = fakeIssuer,
                    OpenIDSubject = "2222222222",
                    Name = "Angelina",
                    Biography = "I'm a ninja cat whisperer with a passion for knitting sweaters for trees."
                });
                database.Users.Add(new User
                {
                    OpenIDIssuer = fakeIssuer,
                    OpenIDSubject = "3333333333",
                    Name = "Will",
                    Biography = "I'm a time-traveling pirate who searches for buried treasure in alternate dimensions."
                });

                database.SaveChanges();
            }
            if (database.Posts.Count() == 0)
            {
            // Add sample posts with tags
            User user1 = database.Users.First(u => u.Name == "Brad");
            User user2 = database.Users.First(u => u.Name == "Angelina");

            var posts = new List<Post>
            {
                new Post
                {
                    Description = "Hello World!",
                    UserID = user1.ID,
                    Tag = "sample"
                },
                new Post
                {
                    Description = "Amazing sunset!",
                    UserID = user2.ID,
                    Tag = "nature"
                },
                new Post
                {
                    Description = "Yummy food!",
                    UserID = user1.ID,
                    Tag = "food"
                }
            };

            database.Posts.AddRange(posts);
            database.SaveChanges();

            }
        }
    }
}

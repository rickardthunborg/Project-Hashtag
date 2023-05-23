using Project_Hashtag.Models;
using System.Collections.Generic;

namespace Project_Hashtag.Data
{
    public class SampleData
    {
        public static void Create(AppDbContext database)
        {
        string fakeIssuer = "https://example.com";
        if (!database.Users.Any(a => a.OpenIDIssuer == fakeIssuer))
        {
            database.Users.Add(new User
            {
                OpenIDIssuer = fakeIssuer,
                OpenIDSubject = "1111111111",
                Name = "Brad",
                Biography = "I'm a pizza-loving astronaut who dreams of playing guitar on Mars.",
                Avatar = "/uploads/standard-profile-pic.png"
            });
            database.Users.Add(new User
            {
                OpenIDIssuer = fakeIssuer,
                OpenIDSubject = "2222222222",
                Name = "Angelina",
                Biography = "I'm a ninja cat whisperer with a passion for knitting sweaters for trees.",
                Avatar = "/uploads/standard-profile-pic.png"
            });
            database.Users.Add(new User
            {
                OpenIDIssuer = fakeIssuer,
                OpenIDSubject = "3333333333",
                Name = "Will",
                Biography = "I'm a time-traveling pirate who searches for buried treasure in alternate dimensions.",
                Avatar = "/uploads/standard-profile-pic.png"
            });
            
            database.Users.Add(new User
            {
                OpenIDIssuer = fakeIssuer,
                OpenIDSubject = "4444444444",
                Name = "Oliver",
                Biography = "I'm a tech-savvy inventor who fights for digital privacy and open-source software.",
                Avatar = "/uploads/standard-profile-pic.png"
            });
            database.Users.Add(new User
            {
                OpenIDIssuer = fakeIssuer,
                OpenIDSubject = "5555555555",
                Name = "Margaret Thatcher",
                Biography = "I am a former Prime Minister who championed conservative principles and free-market economics.",
                Avatar = "/uploads/standard-profile-pic.png"
            });
            database.Users.Add(new User
            {
                OpenIDIssuer = fakeIssuer,
                OpenIDSubject = "6666666666",
                Name = "Che Guevara",
                Biography = "I was a Marxist revolutionary and guerrilla leader, advocating for socialist ideals and armed struggle.",
                Avatar = "/uploads/standard-profile-pic.png"
            });
            database.Users.Add(new User
            {
                OpenIDIssuer = fakeIssuer,
                OpenIDSubject = "7777777777",
                Name = "Nelson Mandela",
                Biography = "I was a South African anti-apartheid revolutionary and politician, fighting for equality and human rights.",
                Avatar = "/uploads/standard-profile-pic.png"
            });
            database.Users.Add(new User
            {
                OpenIDIssuer = fakeIssuer,
                OpenIDSubject = "8888888888",
                Name = "Winston Churchill",
                Biography = "I was a British statesman and Prime Minister during World War II, known for my leadership and rhetoric.",
                Avatar = "/uploads/standard-profile-pic.png"
            });
            database.Users.Add(new User
            {
                OpenIDIssuer = fakeIssuer,
                OpenIDSubject = "9999999999",
                Name = "Karl Marx",
                Biography = "I was a philosopher, economist, and socialist revolutionary, developing the theory of communism.",
                Avatar = "/uploads/standard-profile-pic.png"
            });
            database.Users.Add(new User
            {
                OpenIDIssuer = fakeIssuer,
                OpenIDSubject = "0000000000",
                Name = "Ronald Reagan",
                Biography = "I was an American actor and politician, serving as the 40th President and advocating conservative policies.",
                Avatar = "/uploads/standard-profile-pic.png"
            });
            database.Users.Add(new User
            {
                OpenIDIssuer = fakeIssuer,
                OpenIDSubject = "1234567890",
                Name = "Mahatma Gandhi",
                Biography = "I was an Indian lawyer, anti-colonial nationalist, and spiritual leader, promoting nonviolent resistance.",
                Avatar = "/uploads/standard-profile-pic.png"
            });
            database.Users.Add(new User
            {
                OpenIDIssuer = fakeIssuer,
                OpenIDSubject = "9876543210",
                Name = "Mao Zedong",
                Biography = "I was a Chinese communist revolutionary and founding father of the People's Republic of China.",
                Avatar = "/uploads/standard-profile-pic.png"
            });

            database.SaveChanges();
        }

            // Make all users follow Oliver
            var users = database.Users.ToList();
            var oliver = users.FirstOrDefault(u => u.Name == "Oliver");
            if (oliver != null)
            {
                foreach (var user in users)
                {
                    if (user.ID != oliver.ID && !database.Follows.Any(f => f.UserID == user.ID && f.FollowingId == oliver.ID))
                    {
                        database.Follows.Add(new Follow
                        {
                            UserID = user.ID,
                            FollowingId = oliver.ID
                        });
                    }
                }
                database.Users.Add(new User
                {
                    OpenIDIssuer = fakeIssuer,
                    OpenIDSubject = "1111111111",
                    Name = "Brad",
                    Biography = "I'm a pizza-loving astronaut who dreams of playing guitar on Mars.",
                    Avatar = "/standard-profile-pic.png"
                });
                database.Users.Add(new User
                {
                    OpenIDIssuer = fakeIssuer,
                    OpenIDSubject = "2222222222",
                    Name = "Angelina",
                    Biography = "I'm a ninja cat whisperer with a passion for knitting sweaters for trees.",
                    Avatar = "/standard-profile-pic.png"

                });
                database.Users.Add(new User
                {
                    OpenIDIssuer = fakeIssuer,
                    OpenIDSubject = "3333333333",
                    Name = "Will",
                    Biography = "I'm a time-traveling pirate who searches for buried treasure in alternate dimensions.",
                    Avatar = "/standard-profile-pic.png"
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
                },
                new Post
                {
                    Description = "My first selfie:)!",
                    UserID = user1.ID,
                    Tag = "Selfie",
                    PictureUrl = "/uploads/standard-profile-pic.png"
                }
            };

            database.Posts.AddRange(posts);
            database.SaveChanges();

            }
        }
    }
}

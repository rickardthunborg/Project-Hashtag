using Project_Hashtag.Models;

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
                    Name = "Brad"
                });
                database.Users.Add(new User
                {
                    OpenIDIssuer = fakeIssuer,
                    OpenIDSubject = "2222222222",
                    Name = "Angelina"
                });
                database.Users.Add(new User
                {
                    OpenIDIssuer = fakeIssuer,
                    OpenIDSubject = "3333333333",
                    Name = "Will"
                });
            }

            if (!database.Tags.Any())
            {
                database.Tags.Add(new Tag { Name = "No tag" });

                database.Tags.Add(new Tag { Name = "Food" });

                database.Tags.Add(new Tag { Name = "Travel" });

                database.Tags.Add(new Tag { Name = "Fitness" });

                database.Tags.Add(new Tag { Name = "Music" });

                database.Tags.Add(new Tag { Name = "Fashion" });

                database.Tags.Add(new Tag { Name = "Gaming" });

                database.Tags.Add(new Tag { Name = "Art" });

                database.Tags.Add(new Tag { Name = "Sports" });
            }

            database.SaveChanges();
        }
    }
}

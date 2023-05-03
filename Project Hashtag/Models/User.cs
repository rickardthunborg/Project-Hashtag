﻿namespace Project_Hashtag.Models
{
    public class User
    {
        public int ID { get; set; }
        public string OpenIDIssuer { get; set; }
        public string OpenIDSubject { get; set; }
        public string Name { get; set; }

        public List<Like> Likes { get; set; }
    }
}

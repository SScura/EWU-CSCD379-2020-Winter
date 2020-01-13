using System;

namespace SecretSanta.Business
{
    public class Gift
    {
        public Gift(int id, string title, string description, string url, User user)
        {
            Id = id;
            Title = title;
            Description = description;
            Url = url;
            User = user;
        }
        public int Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public User? User
        {
            get => _User;
            set => _User = value ?? throw new ArgumentNullException(nameof(value));
        }
        private User? _User;
    }
}
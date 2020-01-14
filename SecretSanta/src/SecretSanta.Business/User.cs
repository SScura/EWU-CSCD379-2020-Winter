using System;
using System.Collections.Generic;

namespace SecretSanta.Business
{
    public class User
    {
        public User(int id, string firstName, string lastName, List<Gift> gifts)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Gifts = gifts ?? throw new ArgumentNullException(nameof (gifts));
        }

        public int Id { get; }
        public string? FirstName { get => _FirstName; set => _FirstName = value ?? throw new ArgumentNullException(nameof(value)); }
        public string? LastName { get => _LastName; set => _LastName = value ?? throw new ArgumentNullException(nameof(value)); }
        public List<Gift> Gifts { get; }
        private string? _FirstName;
        private string? _LastName;
    }
}
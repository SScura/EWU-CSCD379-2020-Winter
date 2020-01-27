using System;
using System.Collections.Generic;

namespace SecretSanta.Data
{
    public class User : FingerPrintEntityBase
    {
        public User(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;         
        }

        public string FirstName { get => _FirstName; set => _FirstName = value ?? throw new ArgumentNullException(nameof(FirstName)); }
        public string LastName { get => _LastName; set => _LastName = value ?? throw new ArgumentNullException(nameof(LastName)); }
        private string _LastName = null!;
        private string _FirstName = null!;
        public int? SantaId { get; set; }
        public User? Santa { get; set; }
        public List<Gift> Gifts { get; } = new List<Gift>();
        public List<UserGroup> UserGroups { get; } = new List<UserGroup>();
    }
}
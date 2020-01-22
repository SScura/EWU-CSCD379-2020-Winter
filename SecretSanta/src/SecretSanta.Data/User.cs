using System;
using System.Collections.Generic;

namespace SecretSanta.Data
{
    public class User : FingerPrintEntityBase
    {
        public string FirstName { get => _FirstName; set => _FirstName = value ?? throw new ArgumentNullException(nameof(FirstName)); }
        public string LastName { get => _LastName; set => _LastName = value ?? throw new ArgumentNullException(nameof(LastName)); }
        public User Santa { get; set; }
#nullable disable
        public ICollection<Gift> Gifts { get; set; }
        public List<GroupInfo> GroupsInfo { get; set; }
#nullable enable
        private string _FirstName = string.Empty;
        private string _LastName = string.Empty;
    }
}
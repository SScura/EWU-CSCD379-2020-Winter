using System;
using System.Collections.Generic;

namespace SecretSanta.Data
{
    public class User : FingerPrintEntityBase
    {
        public string FirstName { get => _FirstName; set => _FirstName = value ?? throw new ArgumentNullException(nameof(FirstName)); }
        public string LastName { get => _LastName; set => _LastName = value ?? throw new ArgumentNullException(nameof(LastName)); }
#nullable disable
        public User Santa { get; set; }
        public ICollection<Gift> Gifts { get; set; }
        public List<GroupInfo> GroupsInfo { get; set; }
#nullable enable
        private string _FirstName = string.Empty;
        private string _LastName = string.Empty;
    }
}
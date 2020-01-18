using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Data
{
    public class GroupInfo
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
#nullable disable
        public Group Group { get; set; }
        public User User { get; set; }
#nullable enable
    }
}

using System;
using System.Collections.Generic;

namespace Shared.OL
{
    public class Group
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; }
        public string OwnerUsername { get; set; }
        public List<string> MemberUsernames { get; set; } = new List<string>();

        public Group() { }

        public Group(string name, string owner, List<string> members)
        {
            Name = name;
            OwnerUsername = owner;
            MemberUsernames = members;
        }
    }
}

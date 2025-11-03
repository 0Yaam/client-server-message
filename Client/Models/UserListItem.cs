using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class UserListItem
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string LastMessage { get; set; }
        public System.DateTime? Time { get; set; }
    }
}

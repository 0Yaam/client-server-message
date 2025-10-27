using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiTapNhomMMT.Backend.OL
{
        public class Group
        {
            public string Id { get; set; } = Guid.NewGuid().ToString("N");
            public string Name { get; set; }
            public List<string> MemberIds { get; set; } = new List<string>();

            public Group(string name, List<string> members)
            {
                Name = name;
                MemberIds = members;
            }
        }
}

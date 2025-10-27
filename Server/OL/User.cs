using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiTapNhomMMT.Backend.OL
{
    public class User
    {
        public string Id { get; set; }             // có thể dùng Guid hoặc username
        public string DisplayName { get; set; }    // tên hiển thị
        public string AvatarPath { get; set; }     // ảnh đại diện (opt)

        public User(string id, string name, string avatarPath = null)
        {
            Id = id;
            DisplayName = name;
            AvatarPath = avatarPath;
        }
    }

}

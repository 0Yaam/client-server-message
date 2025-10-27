using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiTapNhomMMT
{
    public class TinNhanItem
    {
        public string NoiDung { get; set; } = "";
        public bool LaNguoiDung { get; set; } // true = bạn, false = đối phương
    }
    public class LienLac
    {
        
        public string TenNguoiDung { get; set; }
        public List<TinNhanItem> TinNhans { get; set; }

        public LienLac(string ten)
        {
            TenNguoiDung = ten;
        }

        public void ThemTinNhan(string noiDung, bool laNguoiDung)
        {
            TinNhans.Add(new TinNhanItem { NoiDung = noiDung, LaNguoiDung = laNguoiDung });
        }
    }
}

using System.Collections.ObjectModel;

namespace MyApp.Models
{
    public class TinNhanItem
    {
        public string NoiDung { get; set; } = "";
        public bool LaNguoiDung { get; set; } // true = bạn, false = đối phương
    }

    public class DanhBa
    {
        public string TenNguoiDung { get; set; }
        public ObservableCollection<TinNhanItem> TinNhans { get; set; } = new();

        public DanhBa(string ten)
        {
            TenNguoiDung = ten;
        }

        public void ThemTinNhan(string noiDung, bool laNguoiDung)
        {
            TinNhans.Add(new TinNhanItem { NoiDung = noiDung, LaNguoiDung = laNguoiDung });
        }
    }
}
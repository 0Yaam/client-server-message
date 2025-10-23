using Avalonia.Controls;
using MyApp.Models;

namespace MyApp
{
    public partial class ChatView : UserControl
    {
        public ChatView()
        {
            InitializeComponent();

            var danhBa = new DanhBa("Nguyễn Văn A");
            danhBa.ThemTinNhan("Xin chào!", false);
            danhBa.ThemTinNhan("Chào bạn!", true);
            danhBa.ThemTinNhan("Hôm nay bạn khỏe không?", false);
            danhBa.ThemTinNhan("Mình khỏe, cảm ơn!", true);

            DataContext = danhBa;
        }
    }
}

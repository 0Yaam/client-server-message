// Client/Views/LoginView.axaml.cs
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client.Views;

public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
    }

    // Tự viết hàm này để nạp XAML nếu source generator chưa tạo
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

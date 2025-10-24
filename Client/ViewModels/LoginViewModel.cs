// Client/ViewModels/LoginViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Client.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty] private string email = "";
    [ObservableProperty] private string password = "";

    [RelayCommand]
    private Task Login()
    {
        // TODO: gọi AuthService.LoginAsync(Email, Password)
        // demo:
        System.Diagnostics.Debug.WriteLine($"Login: {Email}/{Password}");
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task Google()
    {
        // TODO: OAuth flow (nếu có)
        System.Diagnostics.Debug.WriteLine("Google login clicked");
        return Task.CompletedTask;
    }
}

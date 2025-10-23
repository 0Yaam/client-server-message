using Avalonia.Controls;

namespace MyApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void Contact_Click(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        ContentArea.Content = new ChatView();
    }
}
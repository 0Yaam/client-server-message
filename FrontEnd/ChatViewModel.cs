using System.Collections.ObjectModel;
using System.Windows.Input;
using ReactiveUI;

namespace TelegramClone.ViewModels;

public class ChatViewModel : ViewModelBase
{
    public ObservableCollection<MessageViewModel> Messages { get; } = new();
    public string NewMessage { get; set; }

    public ICommand SendCommand => ReactiveCommand.Create(Send);

    private void Send()
    {
        if (!string.IsNullOrWhiteSpace(NewMessage))
        {
            Messages.Add(new MessageViewModel { Text = NewMessage });
            NewMessage = string.Empty;
        }
    }
}

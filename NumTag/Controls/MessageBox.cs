using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace NumTag.Controls;

public class MessageBox : Window
{
    private MessageBox(string message)
    {
        Content = new TextBlock
        {
            Text = message,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(40),
        };
    }

    public static void Show(
        string message,
        string title = "",
        Window? owner = null,
        int minWidth = 300,
        int minHeight = 100)
    {
        var msg = new MessageBox(message)
        {
            MinWidth = minWidth,
            MinHeight = minHeight,
            Title = title,
            ShowActivated = true,
            CanMaximize = false,
            CanMinimize = false,
            ShowInTaskbar = false,
            SizeToContent = SizeToContent.WidthAndHeight,
        };
        if (owner != null)
        {
            msg.Owner = owner;
            msg.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            msg.Icon = owner.Icon;
        }
        else
        {
            msg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        msg.Show();
    }
}

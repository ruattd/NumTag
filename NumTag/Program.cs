using Avalonia;
using NumTag.Controls;

namespace NumTag;

sealed class Program
{
    public static void OnUnhandledException(object ex)
    {
        var msg = $"未捕获的异常:\n\n{ex}";
        MessageBox.Show(msg, "锟斤拷烫烫烫");
    }

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            OnUnhandledException(e.ExceptionObject);
            Environment.Exit(1);
        };
        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            OnUnhandledException(e.Exception);
        };
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}

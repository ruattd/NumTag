using Avalonia;
using NumTag.Core.Controls;
using NumTag.Core.Extensions;

namespace NumTag;

sealed class Program
{
    private static string GetContextTag()
    {
        return Task.CurrentId.Let(id => id == null ? null : $"TSK#{id}")
            ?? Thread.CurrentThread.Name
            ?? $"#{Environment.CurrentManagedThreadId}";
    }

    public static void OnUnhandledException(object ex)
    {
        var context = GetContextTag();
        Console.Error.WriteLine($"Exception in {context}:{Environment.NewLine}{ex}");
        MessageBox.Show($"{context} 抛出了未捕获的异常\n\n详细信息:\n{ex}", "锟斤拷烫烫烫");
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

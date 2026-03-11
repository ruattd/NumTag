using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using NumTag.Views;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Threading;
using NumTag.Core.States;
using NumTag.Models;
using NumTag.ViewModels;

namespace NumTag;

public class App : Application
{
    /// <summary>
    /// The main window in desktop environment.
    /// </summary>
    /// <exception cref="InvalidOperationException">Not initialized</exception>
    public static MainWindow DesktopMainWindow
    {
        get => field ?? throw new InvalidOperationException("Not initialized");
        private set;
    }

    /// <summary>
    /// Global settings.
    /// </summary>
    public static Settings Settings { get; } = Settings.Read();

    /// <summary>
    /// Start shutdown the program.
    /// </summary>
    public static void Shutdown()
    {
        Dispatcher.UIThread.BeginInvokeShutdown(DispatcherPriority.Send);
    }

    /* Framework Implementations */

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            Dispatcher.UIThread.UnhandledException += (_, e) =>
            {
                Program.OnUnhandledException(e.Exception);
                // continue executing
                e.Handled = true;
            };
            DesktopMainWindow = new MainWindow(new MainWindowViewModel { Behavior = new BehaviorState(Settings.MergedBehavior()) });
        }

        base.OnFrameworkInitializationCompleted();
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026")]
    private static void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    // visibility switch
    private void MenuItemVisibility_OnClicked(object? sender, EventArgs e)
    {
        DesktopMainWindow.FlipVisibility();
    }

    // exit
    private void MenuItemExit_OnClick(object? sender, EventArgs e)
    {
        Shutdown();
    }

    // open settings
    private void MenuItemSettings_OnClick(object? sender, EventArgs e)
    {
        DesktopMainWindow.ViewModel.OpenSettings(DesktopMainWindow);
    }
}

using CommunityToolkit.Mvvm.Input;
using NumTag.Core.ViewModels;

namespace NumTag.ViewModels;

public partial class SettingsWindowViewModel : ViewModelBase
{
    public event Action? CloseWindow;

    [RelayCommand]
    public void Save()
    {
        
        App.Settings.SaveAsCurrentBehavior(Behavior.ToSettings());
        App.Settings.Write();
        CloseWindow?.Invoke();
    }

    [RelayCommand]
    public void Cancel()
    {
        Behavior.LoadSettings(App.Settings.MergedBehavior());
        CloseWindow?.Invoke();
    }
}

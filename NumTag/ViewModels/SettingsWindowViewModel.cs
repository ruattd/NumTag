using CommunityToolkit.Mvvm.Input;
using NumTag.Core.ViewModels;

namespace NumTag.ViewModels;

public partial class SettingsWindowViewModel : ViewModelBase
{
    public event Action? CloseWindow;

    [RelayCommand]
    public void Save()
    {
        // TODO write vm value to settings
        App.Settings.Write();
        CloseWindow?.Invoke();
    }

    public void Cancel()
    {
        // TODO restore vm value
        CloseWindow?.Invoke();
    }
}

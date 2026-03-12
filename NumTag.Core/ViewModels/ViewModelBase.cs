using CommunityToolkit.Mvvm.ComponentModel;
using NumTag.Core.States;

namespace NumTag.Core.ViewModels;

public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private BehaviorState _behavior = new();
}

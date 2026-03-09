using CommunityToolkit.Mvvm.ComponentModel;
using NumTag.Core.States;

namespace NumTag.Core.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    public required BehaviorState Behavior { get; init; }
}

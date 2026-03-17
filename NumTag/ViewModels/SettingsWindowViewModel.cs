using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NumTag.Core.ViewModels;
using NumTag.Models;

namespace NumTag.ViewModels;

public partial class SettingsWindowViewModel : ViewModelBase
{
    public event Action? CloseWindow;

    private const string DefaultSlotName = "默认";

    [ObservableProperty] private string _currentBehaviorSlot = App.Settings.CurrentBehaviorSlot ?? DefaultSlotName;
    partial void OnCurrentBehaviorSlotChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            CurrentBehaviorSlot = DefaultSlotName;
            return;
        }
        if (value == DefaultSlotName) value = null!;
        SwitchSlot(value);
    }

    [ObservableProperty] private string _addingBehaviorSlot = "";
    [ObservableProperty] private bool _isAddingBehaviorSlot = false;
    partial void OnIsAddingBehaviorSlotChanged(bool value)
    {
        if (!value) AddingBehaviorSlot = "";
    }

    public AvaloniaList<string> BehaviorSlots { get; } =
        [DefaultSlotName, ..Settings.BehaviorSlots().OrderBy(x => x, StringComparer.CurrentCultureIgnoreCase)];

    private void SwitchSlot(string? slot)
    {
        // default to null
        if (string.IsNullOrWhiteSpace(slot) || slot == DefaultSlotName) slot = null;
        // ignore same value
        if (App.Settings.CurrentBehaviorSlot == slot) return;
        // save current slot
        Save();
        // switch slot
        if (slot == null) App.Settings.CurrentBehaviorSlot = null;
        else
        {
            var current = App.Settings.MergedBehavior();
            App.Settings.CurrentBehaviorSlot = slot;
            if (!BehaviorSlots.Contains(slot))
            {
                App.Settings.SaveAsCurrentBehavior(current);
                BehaviorSlots.Add(slot);
            }
        }
        App.Settings.Write();
        Reload();
    }

    [RelayCommand]
    private void Save()
    {
        App.Settings.SaveAsCurrentBehavior(Behavior.ToSettings());
        App.Settings.Write();
    }

    [RelayCommand]
    private void SaveAndClose()
    {
        Save();
        CloseWindow?.Invoke();
    }

    [RelayCommand]
    private void StartAddSlot() => IsAddingBehaviorSlot = true;

    [RelayCommand]
    private void ConfirmAddSlot()
    {
        var slot = AddingBehaviorSlot;
        CancelAddSlot();
        if (string.IsNullOrWhiteSpace(slot)) slot = DefaultSlotName;
        CurrentBehaviorSlot = slot;
    }

    [RelayCommand]
    private void CancelAddSlot() => IsAddingBehaviorSlot = false;

    [RelayCommand]
    private void DeleteSlot(string? slot)
    {
        // ignore default
        if (slot is null or DefaultSlotName) return;
        // ignore not existing
        if (!BehaviorSlots.Contains(slot)) return;
        // delete
        if (slot == CurrentBehaviorSlot) CurrentBehaviorSlot = DefaultSlotName;
        Settings.DeleteBehaviorSlot(slot);
        BehaviorSlots.Remove(slot);
    }

    [RelayCommand]
    private void Reload()
    {
        Behavior.LoadSettings(App.Settings.MergedBehavior());
    }

    [RelayCommand]
    private void Cancel() => Reload();

    [RelayCommand]
    private void CancelAndClose()
    {
        Cancel();
        CloseWindow?.Invoke();
    }

    internal void OnKeyUp(Window _, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (IsAddingBehaviorSlot) ConfirmAddSlot();
            else SaveAndClose();
        }
        else if (e.Key == Key.Escape) CancelAndClose();
    }
}

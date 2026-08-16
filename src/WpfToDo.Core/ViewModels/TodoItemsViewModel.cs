using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfToDo.Core.Models;
using WpfToDo.Core.Services;
using WpfToDo.Core.Wrappers;

namespace WpfToDo.Core.ViewModels;

public partial class TodoItemsViewModel : ObservableObject
{
    private readonly ITodoItemService _todoItemService;

    [ObservableProperty]
    private ObservableCollection<TodoItemWrapper> _items = new();

    [ObservableProperty]
    private TodoItemWrapper? _selectedItem;

    [ObservableProperty]
    private string _newItemTitle = string.Empty;

    [ObservableProperty]
    private int? _todoListId;

    public TodoItemsViewModel(ITodoItemService todoItemService)
    {
        _todoItemService = todoItemService;
    }

    public async Task LoadAsync(int todoListId, CancellationToken cancellationToken = default)
    {
        TodoListId = todoListId;
        Items.Clear();
        var items = await _todoItemService.GetByListIdAsync(todoListId, cancellationToken);
        foreach (var item in items)
            Items.Add(new TodoItemWrapper(item));
    }

    public async Task<TodoItemWrapper?> AddItemAsync(
        string? title = null,
        CancellationToken cancellationToken = default)
    {
        var itemTitle = (title ?? NewItemTitle).Trim();
        if (string.IsNullOrWhiteSpace(itemTitle) || TodoListId is null)
            return null;

        var item = await _todoItemService.AddAsync(
            new TodoItem { TodoListId = TodoListId.Value, Title = itemTitle },
            cancellationToken);
        var wrapper = new TodoItemWrapper(item);
        Items.Add(wrapper);
        SelectedItem = wrapper;
        NewItemTitle = string.Empty;
        return wrapper;
    }

    public async Task<bool> UpdateItemAsync(
        TodoItemWrapper wrapper,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(wrapper);
        await _todoItemService.UpdateAsync(wrapper.Model, cancellationToken);
        wrapper.AcceptChanges();
        return true;
    }

    public async Task<bool> ToggleCompletedAsync(
        TodoItemWrapper wrapper,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(wrapper);
        var updated = await _todoItemService.ToggleCompletedAsync(wrapper.Id, cancellationToken);
        if (updated is null)
            return false;

        ReplaceWrapper(wrapper, new TodoItemWrapper(updated));
        return true;
    }

    public async Task<bool> DeleteItemAsync(
        TodoItemWrapper? item = null,
        CancellationToken cancellationToken = default)
    {
        var target = item ?? SelectedItem;
        if (target is null || !await _todoItemService.DeleteAsync(target.Id, cancellationToken))
            return false;

        Items.Remove(target);
        if (ReferenceEquals(SelectedItem, target))
            SelectedItem = null;
        return true;
    }

    [RelayCommand]
    private Task AddItemFromInputAsync() => AddItemAsync();

    [RelayCommand]
    private Task ToggleSelectedItemAsync() =>
        SelectedItem is null ? Task.CompletedTask : ToggleCompletedAsync(SelectedItem);

    [RelayCommand]
    private Task DeleteSelectedItemAsync() => DeleteItemAsync();

    private void ReplaceWrapper(TodoItemWrapper oldWrapper, TodoItemWrapper newWrapper)
    {
        var index = Items.IndexOf(oldWrapper);
        if (index < 0)
            return;

        Items[index] = newWrapper;
        if (ReferenceEquals(SelectedItem, oldWrapper))
            SelectedItem = newWrapper;
    }
}

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfToDo.Core.Models;
using WpfToDo.Core.Services;
using WpfToDo.Core.Wrappers;

namespace WpfToDo.Core.ViewModels;

public partial class TodoListsViewModel : ObservableObject
{
    private readonly ITodoListService _todoListService;

    [ObservableProperty]
    private ObservableCollection<TodoListWrapper> _lists = new();

    [ObservableProperty]
    private TodoListWrapper? _selectedList;

    [ObservableProperty]
    private string _newListName = string.Empty;

    public TodoListsViewModel(ITodoListService todoListService)
    {
        _todoListService = todoListService;
    }

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        Lists.Clear();
        var lists = await _todoListService.GetAllAsync(cancellationToken);
        foreach (var list in lists)
            Lists.Add(new TodoListWrapper(list));
    }

    public async Task<TodoListWrapper?> AddListAsync(
        string? name = null,
        int? categoryId = null,
        CancellationToken cancellationToken = default)
    {
        var listName = (name ?? NewListName).Trim();
        if (string.IsNullOrWhiteSpace(listName))
            return null;

        var list = await _todoListService.AddAsync(
            new TodoList { Name = listName, CategoryId = categoryId },
            cancellationToken);
        var wrapper = new TodoListWrapper(list);
        Lists.Add(wrapper);
        SelectedList = wrapper;
        NewListName = string.Empty;
        return wrapper;
    }

    public async Task<bool> DeleteListAsync(
        TodoListWrapper? list = null,
        CancellationToken cancellationToken = default)
    {
        var target = list ?? SelectedList;
        if (target is null || !await _todoListService.DeleteAsync(target.Id, cancellationToken))
            return false;

        Lists.Remove(target);
        if (ReferenceEquals(SelectedList, target))
            SelectedList = null;
        return true;
    }

    public async Task<TodoListWrapper> UpdateListAsync(
        TodoListWrapper wrapper,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(wrapper);
        var updated = await _todoListService.UpdateAsync(wrapper.Model, cancellationToken);
        wrapper.AcceptChanges();
        return new TodoListWrapper(updated);
    }

    [RelayCommand]
    private Task AddListFromInputAsync() => AddListAsync();

    [RelayCommand]
    private Task DeleteSelectedListAsync() => DeleteListAsync();
}

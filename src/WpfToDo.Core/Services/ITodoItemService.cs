using WpfToDo.Core.Models;

namespace WpfToDo.Core.Services;

public interface ITodoItemService
{
    Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TodoItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TodoItem>> GetByListIdAsync(int todoListId, CancellationToken cancellationToken = default);
    Task<TodoItem> AddAsync(TodoItem todoItem, CancellationToken cancellationToken = default);
    Task<TodoItem> UpdateAsync(TodoItem todoItem, CancellationToken cancellationToken = default);
    Task<TodoItem?> ToggleCompletedAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

using WpfToDo.Core.Models;

namespace WpfToDo.Core.Services;

public interface ITodoListService
{
    Task<IReadOnlyList<TodoList>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TodoList?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TodoList> AddAsync(TodoList todoList, CancellationToken cancellationToken = default);
    Task<TodoList> UpdateAsync(TodoList todoList, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

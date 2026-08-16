using Microsoft.EntityFrameworkCore;
using WpfToDo.Core.Data;
using WpfToDo.Core.Models;

namespace WpfToDo.Core.Services;

public class TodoItemService : ITodoItemService
{
    private readonly WpfToDoDbContext _dbContext;

    public TodoItemService(WpfToDoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.TodoItems
            .AsNoTracking()
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

    public Task<TodoItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _dbContext.TodoItems
            .AsNoTracking()
            .Include(x => x.TodoList)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<TodoItem>> GetByListIdAsync(
        int todoListId,
        CancellationToken cancellationToken = default)
        => await _dbContext.TodoItems
            .AsNoTracking()
            .Where(x => x.TodoListId == todoListId)
            .OrderBy(x => x.IsCompleted)
            .ThenBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<TodoItem> AddAsync(TodoItem todoItem, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(todoItem);
        _dbContext.TodoItems.Add(todoItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return todoItem;
    }

    public async Task<TodoItem> UpdateAsync(TodoItem todoItem, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(todoItem);
        _dbContext.TodoItems.Update(todoItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return todoItem;
    }

    public async Task<TodoItem?> ToggleCompletedAsync(int id, CancellationToken cancellationToken = default)
    {
        var todoItem = await _dbContext.TodoItems.FindAsync([id], cancellationToken);
        if (todoItem is null)
            return null;

        todoItem.IsCompleted = !todoItem.IsCompleted;
        todoItem.CompletedAt = todoItem.IsCompleted ? DateTime.UtcNow : null;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return todoItem;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var todoItem = await _dbContext.TodoItems.FindAsync([id], cancellationToken);
        if (todoItem is null)
            return false;

        _dbContext.TodoItems.Remove(todoItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}

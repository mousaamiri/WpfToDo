using Microsoft.EntityFrameworkCore;
using WpfToDo.Core.Data;
using WpfToDo.Core.Models;

namespace WpfToDo.Core.Services;

public class TodoListService : ITodoListService
{
    private readonly WpfToDoDbContext _dbContext;

    public TodoListService(WpfToDoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<TodoList>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.TodoLists
            .AsNoTracking()
            .Include(x => x.Category)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

    public Task<TodoList?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _dbContext.TodoLists
            .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.Items)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<TodoList> AddAsync(TodoList todoList, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(todoList);
        _dbContext.TodoLists.Add(todoList);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return todoList;
    }

    public async Task<TodoList> UpdateAsync(TodoList todoList, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(todoList);
        _dbContext.TodoLists.Update(todoList);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return todoList;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var todoList = await _dbContext.TodoLists.FindAsync([id], cancellationToken);
        if (todoList is null)
            return false;

        _dbContext.TodoLists.Remove(todoList);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}

using Microsoft.EntityFrameworkCore;
using WpfToDo.Core.Data;
using WpfToDo.Core.Models;

namespace WpfToDo.Core.Services;

public class CategoryService : ICategoryService
{
    private readonly WpfToDoDbContext _dbContext;

    public CategoryService(WpfToDoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Categories
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

    public Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _dbContext.Categories
            .AsNoTracking()
            .Include(x => x.TodoLists)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Category> AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(category);
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task<Category> UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(category);
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _dbContext.Categories.FindAsync([id], cancellationToken);
        if (category is null)
            return false;

        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}

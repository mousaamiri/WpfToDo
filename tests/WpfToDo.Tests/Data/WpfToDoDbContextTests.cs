using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WpfToDo.Core.Data;
using WpfToDo.Core.Models;

namespace WpfToDo.Tests.Data;

public class WpfToDoDbContextTests
{
    [Fact]
    public async Task SqliteContext_CanPersistAndReadRelatedEntities()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<WpfToDoDbContext>()
            .UseSqlite(connection)
            .Options;

        await using (var context = new WpfToDoDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
            var category = new Category { Name = "Personal", ColorKey = "Blue" };
            var list = new TodoList { Name = "Errands", Category = category };
            context.TodoLists.Add(list);
            await context.SaveChangesAsync();
        }

        await using (var context = new WpfToDoDbContext(options))
        {
            var list = await context.TodoLists
                .Include(x => x.Category)
                .SingleAsync();

            list.Name.Should().Be("Errands");
            list.Category.Should().NotBeNull();
            list.Category!.Name.Should().Be("Personal");
        }
    }
}

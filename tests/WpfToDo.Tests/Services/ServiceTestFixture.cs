using Microsoft.EntityFrameworkCore;
using WpfToDo.Core.Data;

namespace WpfToDo.Tests.Services;

internal static class ServiceTestFixture
{
    public static WpfToDoDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<WpfToDoDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new WpfToDoDbContext(options);
    }
}

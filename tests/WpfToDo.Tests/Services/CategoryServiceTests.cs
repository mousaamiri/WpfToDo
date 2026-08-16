using FluentAssertions;
using WpfToDo.Core.Models;
using WpfToDo.Core.Services;

namespace WpfToDo.Tests.Services;

public class CategoryServiceTests
{
    [Fact]
    public async Task CrudAsync_PersistsAndUpdatesCategory()
    {
        await using var context = ServiceTestFixture.CreateContext();
        var service = new CategoryService(context);
        var category = await service.AddAsync(new Category { Name = "Personal", ColorKey = "Blue" });

        category.Name = "Home";
        await service.UpdateAsync(category);
        var result = await service.GetByIdAsync(category.Id);

        result!.Name.Should().Be("Home");
        (await service.DeleteAsync(category.Id)).Should().BeTrue();
        (await service.GetByIdAsync(category.Id)).Should().BeNull();
    }
}

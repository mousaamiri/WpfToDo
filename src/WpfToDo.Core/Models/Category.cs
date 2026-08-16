namespace WpfToDo.Core.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ColorKey { get; set; } = string.Empty;
    public ICollection<TodoList> TodoLists { get; set; } = new List<TodoList>();
}

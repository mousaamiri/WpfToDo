namespace WpfToDo.Core.Models;

public class TodoList
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<TodoItem> Items { get; set; } = new List<TodoItem>();
}

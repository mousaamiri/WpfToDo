namespace WpfToDo.Core.Models;

public class TodoItem
{
    public int Id { get; set; }
    public int TodoListId { get; set; }
    public TodoList? TodoList { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}

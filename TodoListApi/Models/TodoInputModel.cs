namespace TodoListApi.Models
{
  public class TodoInputModel
  {
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required Guid UserId {get; set;}
  }

  public class TodoChangeModel
  {
    public required Guid UserId { get; set; }
  }
}
namespace TodoListApi.Entities
{
  public class Todo
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid UserId { get; set; }

    public Todo(int id, string title, string description, Guid userId)
    {
      Id = id;
      Title = title;
      Description = description;
      UserId = userId;
    }

    public void Update(string title, string description)
    {
      Title = title;
      Description = description;
    }
  }
}
namespace TodoListApi.Entities
{
  public class Todo
  {
    public int id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public void Update(string title, string description)
    {
      Title = title;
      Description = description;
    }
  }
}
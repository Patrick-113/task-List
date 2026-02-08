namespace TodoListApi.Models
{
  public class TodoViewModel
  {
    public int id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Complete { get; set; }
  }
}
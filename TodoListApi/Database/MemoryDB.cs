using TodoListApi.Entities;

namespace TodoListApi.Database
{
  public class MemoryDB
  {
    public List<Todo> TodoMemory { get; } = new();
    public List<User> UserMemory { get; set; }
  }
}
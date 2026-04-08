using Microsoft.EntityFrameworkCore;
using TodoListApi.Entities;

namespace TodoListApi.Database
{
  public class TodoDbContext : DbContext
  {
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }
    public DbSet<Todo> Todos {get; set; }
  }
}
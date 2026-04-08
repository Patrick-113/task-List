using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApi.Database;
using TodoListApi.Entities;
using TodoListApi.Models;

namespace TodoListApi.Controllers
{
  // [Authorize]
  [Route("/todos")]
  [ApiController]
  public class TodoController : ControllerBase
  {
    private readonly TodoDbContext _context;
    public TodoController(TodoDbContext context)
    {
      _context = context;
    }

    [HttpGet]
    public IActionResult GetAll(TodoChangeModel input)
    {
      var load = _context.Todos.Where(t => t.UserId == input.UserId).ToList();
      return Ok(load);
    }

    [HttpPost]
    public IActionResult Post(TodoInputModel input)
    {
      int id = (_context.Todos.Count(t => t.UserId == input.UserId) == 0) ? 1 : _context.Todos.Last(t => t.UserId == input.UserId).Id + 1;

      Todo item = new Todo(id, input.Title, input.Description, input.UserId);
      _context.Todos.Add(item);
      _context.SaveChanges();
      return Created("/todos", input);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, TodoInputModel input)
    {
      var todo = _context.Todos.SingleOrDefault(t => t.Id == id && t.UserId == input.UserId);
      if (todo == null)
      {
        return NotFound();
      }

      todo.Update(input.Title, input.Description);
      _context.Todos.Update(todo);
      _context.SaveChanges();

      return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult Patch(int id, TodoChangeModel input)
    {
      var todo = _context.Todos.SingleOrDefault(t => t.Id == id && t.UserId == input.UserId);
      if (todo == null)
      {
        return NotFound();
      }

      todo.UpdateStatus();
      _context.Todos.Update(todo);
      _context.SaveChanges();

      return Ok(todo);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id, TodoChangeModel input)
    {
      var todo = _context.Todos.SingleOrDefault(t => t.Id == id && t.UserId == input.UserId);
      if (todo == null)
      {
        return NotFound();
      }

      _context.Todos.Remove(todo);
      _context.SaveChanges();

      return NoContent();
    }
  }
}
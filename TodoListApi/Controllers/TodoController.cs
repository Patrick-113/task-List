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
    private readonly MemoryDB _memory;
    public TodoController(MemoryDB memory)
    {
      _memory = memory;
    }

    [HttpGet]
    public IActionResult GetAll(TodoChangeModel input)
    {
      var load = _memory.TodoMemory.Where(t => t.UserId == input.UserId).ToList();
      return Ok(load);
    }

    [HttpPost]
    public IActionResult Post(TodoInputModel input)
    {
      int id = (_memory.TodoMemory.Count(t => t.UserId == input.UserId) == 0) ? 1 : _memory.TodoMemory.Last(t => t.UserId == input.UserId).Id + 1;

      Todo item = new Todo(id, input.Title, input.Description, input.UserId);
      _memory.TodoMemory.Add(item);
      return Created("/todos", input);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, TodoInputModel input)
    {
      var todo = _memory.TodoMemory.SingleOrDefault(t => t.Id == id && t.UserId == input.UserId);
      if (todo == null)
      {
        return NotFound();
      }

      todo.Update(input.Title, input.Description);
      return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult Patch(int id, TodoChangeModel input)
    {
      var todo = _memory.TodoMemory.SingleOrDefault(t => t.Id == id && t.UserId == input.UserId);
      if (todo == null)
      {
        return NotFound();
      }

      todo.UpdateStatus();
      return Ok(todo);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id, Guid userId)
    {
      var todo = _memory.TodoMemory.SingleOrDefault(t => t.Id == id && t.UserId == userId);
      if (todo == null)
      {
        return NotFound();
      }

      _memory.TodoMemory.Remove(todo);
      return NoContent();
    }
  }
}
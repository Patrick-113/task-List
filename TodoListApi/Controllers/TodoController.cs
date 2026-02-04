using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApi.Database;
using TodoListApi.Entities;

namespace TodoListApi.Controllers
{
  [Authorize]
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
    public IActionResult GetAll()
    {
      if (_memory.TodoMemory == null)
      {
        return NotFound();
      }

      return Ok(_memory.TodoMemory);
    }

    [HttpPost]
    public IActionResult Post(Todo input)
    {
      _memory.TodoMemory.Add(input);
      return Created("/todos", input);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, Todo input)
    {
      var todo = _memory.TodoMemory.SingleOrDefault(t => t.id == id);
      if (todo == null)
      {
        return NotFound();
      }

      todo.Update(input.Title, input.Description);
      return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      var todo = _memory.TodoMemory.SingleOrDefault(t => t.id == id);
      if (todo == null)
      {
        return NotFound();
      }

      _memory.TodoMemory.Remove(todo);
      return NoContent();
    }
  }
}
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

    /// <summary>
    /// Método Get que devolve todas as tasks de certo usuário
    /// </summary>
    /// <param name="userId">Identificador do usuário</param>
    /// <returns>Lista de tarefas do usuário</returns>
    /// <response code="200">Sucesso em buscar a lista de tarefas</response>
    /// <response code="500">Falhar interna do server</response>
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetAll(string userId)
    {
      var load = _context.Todos.Where(t => t.UserId == Guid.Parse(userId.Trim())).ToList();
      return Ok(load);
    }

    /// <summary>
    /// Método para criação de novas tarefas
    /// </summary>
    /// <param name="input">Dados das tarefas</param>
    /// <returns>A tarefa criada</returns>
    /// <response code="201">Sucesso em buscar a lista de tarefas</response>
    /// <response code="500">Falhar interna do server</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Post(TodoInputModel input)
    {
      int id = (_context.Todos.Count(t => t.UserId == input.UserId) == 0) ? 1 : _context.Todos.Last(t => t.UserId == input.UserId).Id + 1;

      Todo item = new Todo(id, input.Title, input.Description, input.UserId);
      _context.Todos.Add(item);
      _context.SaveChanges();
      return Created("/todos", input);
    }

    /// <summary>
    /// Mudança no nome e descrição de tarefas
    /// </summary>
    /// <param name="id">Identificador da tarefa</param>
    /// <param name="input">Dados da tarefa a ser modificada</param>
    /// <response code="204">Sucesso na modificação da tarefa</response>
    /// <response code="404">Tarefa não encontrada</response>
    /// <response code="500">Falhar interna do server</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Mudança do status de tarefas
    /// </summary>
    /// <param name="id">Identificador da tarefa</param>
    /// <param name="userId">Identificador do usuário</param>
    /// <returns>Tarefa modificada.</returns>
    /// <response code="200">Sucesso na modificação do status da tarefa</response>
    /// <response code="404">Tarefa não encontrada</response>
    /// <response code="500">Falhar interna do server</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Patch(int id, string userId)
    {
      var todo = _context.Todos.SingleOrDefault(t => t.Id == id && t.UserId == Guid.Parse(userId.Trim()));
      if (todo == null)
      {
        return NotFound();
      }

      todo.UpdateStatus();
      _context.Todos.Update(todo);
      _context.SaveChanges();

      return Ok(todo);
    }

    /// <summary>
    /// Deleta tarefa por id
    /// </summary>
    /// <param name="id">Identificador da tarefa</param>
    /// <param name="userId">Identificador do usuário</param>
    /// <response code="204">Sucesso ao deletar a tarefa</response>
    /// <response code="404">Tarefa não encontrada</response>
    /// <response code="500">Falhar interna do server</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Delete(int id, string userId)
    {
      var todo = _context.Todos.SingleOrDefault(t => t.Id == id && t.UserId == Guid.Parse(userId.Trim()));
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
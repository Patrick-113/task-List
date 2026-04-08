using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using TodoListApi.Authentication;
using TodoListApi.Database;
using TodoListApi.Entities;
using TodoListApi.Models;

namespace TodoListApi.Controllers
{
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly TodoDbContext _context;
    private readonly TokenService _tokenService;
    public UserController(TokenService tokenService, TodoDbContext context)
    {
      _tokenService = tokenService;
      _context = context;
    }

    /// <summary>
    /// Listar as contas que existem para propósitos de teste
    /// </summary>
    /// <returns>Lista de contas de usuário</returns>
    /// <response code="200">Sucesso em buscar a lista de usuários</response>
    /// <response code="500">Falhar interna do server</response>
    [Route("/account")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetAll()
    {
      return Ok(_context.Users);
    }

    /// <summary>
    /// Registro de novo usuário
    /// </summary>
    /// <param name="input">Informações do usuário</param>
    /// <returns>Identificador do usuário e Token de Acesso</returns>
    /// <response code="200">Sucesso em criar um novo usuário</response>
    /// <response code="500">Falhar interna do server</response>
    [Route("/account/register")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Register(RegisterInputModel input)
    {
      //Encodificação da senha no padrão SHA256
      var hash = SHA256.Create();
      var Password = hash.ComputeHash(Encoding.UTF8.GetBytes(input.Password));
      User user = new User(
        input.Name,
        input.Email,
        Password
      );

      _context.Users.Add(user);
      _context.SaveChanges();

      var load = new UserViewModel(user.Id, _tokenService.GenerateToken(user.Name));

      return Ok(load);
    }

    /// <summary>
    /// Login de um usuário já existente
    /// </summary>
    /// <param name="input">Email e senha do usuário</param>
    /// <returns>Identificador do usuário e Token de Acesso</returns>
    /// <response code="200">Sucesso em efetuar login</response>
    /// <response code="404">Usuário não encontrado</response>
    /// <response code="500">Falhar interna do server</response>
    [Route("/account/login")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Login(LoginInputModel input)
    {
      var hash = SHA256.Create();
      var _password = hash.ComputeHash(Encoding.UTF8.GetBytes(input.Password));
      var user = _context.Users.SingleOrDefault(u => u.Email == input.Email);

      if (user == null)
      {
        return NotFound();
      } else if (user.Password.SequenceEqual(_password))
      {
        var load = new UserViewModel(user.Id, _tokenService.GenerateToken(user.Name));
        return Ok(load);
      } else
      {
        return NotFound();
      }
    }
  }
}
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

    [Route("/account")]
    [HttpGet]
    public IActionResult GetAll()
    {
      return Ok(_context.Users);
    }

    [Route("/account/register")]
    [HttpPost]
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

    [Route("/account/login")]
    [HttpPost]
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
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
    private readonly MemoryDB _memory;
    private readonly TokenService _tokenService;
    public UserController(MemoryDB memory, TokenService tokenService)
    {
      _memory = memory;
      _tokenService = tokenService;
    }

    [Route("/account")]
    [HttpGet]
    public IActionResult GetAll()
    {
      return Ok(_memory.UserMemory);
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
      _memory.UserMemory.Add(user);

      var token = _tokenService.GenerateToken(user.Name);

      return Ok(new {Token = token});
    }

    [Route("/account/login")]
    [HttpPost]
    public IActionResult Login(LoginInputModel input)
    {
      var hash = SHA256.Create();
      var _password = hash.ComputeHash(Encoding.UTF8.GetBytes(input.Password));
      var user = _memory.UserMemory.SingleOrDefault(u => u.Email == input.Email);

      if (user == null)
      {
        return NotFound();
      } else if (user.Password.SequenceEqual(_password))
      {
        var token = _tokenService.GenerateToken(user.Name);
        return Ok(new {Token = token});
      } else
      {
        return NotFound();
      }
    }
  }
}
namespace TodoListApi.Models
{
  public class RegisterInputModel
  {
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
  }

  public class LoginInputModel
  {
    public string Email { get; set; }
    public string Password { get; set; }
  }
}
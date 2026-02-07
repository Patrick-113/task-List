namespace TodoListApi.Models
{
  public class UserViewModel
  {
    public Guid UserId { get; set; }
    public string Token { get; set; }

    public UserViewModel(Guid userID, string token)
    {
      UserId = userID;
      Token = token;
    }
  }
}
namespace TodoListApi.Entities
{
  public class User
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public byte[] Password { get; set; }

    public User(string name, string email, byte[] password)
    {
      Id = Guid.NewGuid();
      Name = name;
      Email = email;
      Password = password;
    }
  }
}
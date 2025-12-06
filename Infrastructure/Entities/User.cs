
namespace Infrastructure.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }

        public override string ToString()
        {
            return string.Format("[Id = {0}, Username = {1}, Name = {2}, Email = {3}, Password = {4}, Active = {5}]",
                Id, UserName, Name, Email, Password, Active);
        }
    }
}
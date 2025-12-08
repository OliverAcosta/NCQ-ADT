
namespace Infrastructure.Entities
{
    public class Priorities
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public override string ToString()
        {
            return string.Format("[Id = {0}, Name = {1}, Description = {2}, Active = {3}]", Id, Name, Description, Active);
        }
    }
}

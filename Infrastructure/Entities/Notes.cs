
namespace Infrastructure.Entities
{
    public class Notes
    {
        public int Id { get; set; }
        public int UserTaskId { get; set; }
        public string Note { get; set; }
        public int Active { get; set; }

        public override string ToString()
        {
            return string.Format("[Id = {0}, UsertaskId = {1}, Note = {2}, Active = {3}]", Id, UserTaskId, Note, Active);
        }
    }
}

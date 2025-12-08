

namespace Infrastructure.Entities
{
    public class UserTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public int PriorityId { get; set; }
        public DateTime Created { get; set; }
        public DateTime DueDate { get; set; }
        public bool Active { get; set; }
    }
}

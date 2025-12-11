

namespace Infrastructure.Entities
{
    public class UserTask:IEquatable<UserTask>
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


        public bool Equals(UserTask other)
        {
            return (this.Id == other.Id && this.Description == other.Description &&
                this.UserId == other.UserId && this.StatusId == other.StatusId && this.PriorityId == other.PriorityId 
                && this.Active == other.Active && this.DueDate == other.DueDate);
        }

        public override string ToString()
        {
            return string.Format("[Id={0}, Name={1}, Description={2}, UserId={3}, StatusId={4}, PriorityId={5}, Created={6}, Duedate={7}, Active={8}]", 
                Id, Name, Description, UserId, StatusId, PriorityId, Created, DueDate, Active);
        }
    }
}

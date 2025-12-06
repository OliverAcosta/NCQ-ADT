using Infrastructure.Entities;

namespace Infrastructure.Dtos
{
    public class UserTaskDto:UserTask
    {
        public string Username { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public List<string> Notes { get; set; }
    }
}

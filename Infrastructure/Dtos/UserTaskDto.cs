using Infrastructure.Entities;

namespace Infrastructure.Dtos
{
    public class UserTaskDto:UserTask
    {
        public string Username { get; set; }
        public string User { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return $"[Id={Id}, Name = '{Name}', Description = '{Description}', UserId = {UserId}'," +
                $"StatusId = {StatusId}, PriorityId = {PriorityId}, Username = '{Username}', User = '{User}', Status = '{Status}', Priority = {Priority}]";
        }
    }
}

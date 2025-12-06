using Infrastructure.Dal.interfaces;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using System.Data.SQLite;
using static Dapper.SqlMapper;

namespace Infrastructure.Dal.Repositories
{
    public class UserTaskRepository : IRepository<UserTask>, IRangeRespository<UserTask>
    {
       
        public void Add(UserTask entity)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                connection.Execute(@"Insert into UserTask(Name, Description, UserId, StatusId, PriorityId, Created, DueDate)
                  values (@name, @description, @userid, @statusId, @priorityId, @created, @dueDate)", 
                  new { entity.Name, entity.Description, entity.UserId, entity.StatusId, entity.PriorityId, DateTime.Now, entity.DueDate });
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Execute(@"Delete from UserTask where Id = @id", new { id }) > 0;
            }
        }

        public UserTask Get(int id)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<UserTask>(@"Select Id, Name, Description, UserId, StatusId, PriorityId, Created, DueDate from User where Id = @id", new { id });
            }
        }

        public IEnumerable<UserTask> GetRange(int[] range)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Query<UserTask>(@"Select Id, Name, Description, UserId, StatusId, PriorityId, Created, DueDate from UserTask where Id in @range", range);
            }
        }

        public IEnumerable<UserTask> All()
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Query<UserTask>(@"Select Id, Name, Description, UserId, StatusId, PriorityId, Created, DueDate from User");
            }
        }
        public void Update(UserTask entity)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                connection.QueryFirstOrDefault(@"Update User set Name = @name, Description = @description, UserId = @userid, StatusId = @statusId,
                                                PriorityId = @priorityId, DueDate = @duedate where Id = @id",
                    new { entity.Id, entity.Name, entity.Description, entity.UserId, entity.StatusId, entity.PriorityId, entity.DueDate});
            }
        }

        public IEnumerable<UserTaskDto> getDTOs()
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Query<UserTaskDto>(@"Select Id, Name, Description, UserId, StatusId, PriorityId, Created, DueDate from User");
            }
        }
    }
}

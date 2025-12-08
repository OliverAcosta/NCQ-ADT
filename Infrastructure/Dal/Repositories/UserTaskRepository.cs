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
                connection.Execute(@"Insert into UserTask(Name, Description, UserId, StatusId, PriorityId, Created, DueDate, Active)
                  values (@Name, @Description, @UserId, @StatusId, @PriorityId, @Created, @DueDate, @Active);", 
                  new { entity.Name, entity.Description, entity.UserId, entity.StatusId, entity.PriorityId, entity.Created, entity.DueDate, entity.Active });
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
                return connection.QueryFirstOrDefault<UserTask>(@"Select Id, Name, Description, UserId, StatusId, PriorityId, Created, DueDate, Active from UserTask where Id = @id", new { id });
            }
        }

        public IEnumerable<UserTask> GetRange(int[] range)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Query<UserTask>(@"Select Id, Name, Description, UserId, StatusId, PriorityId, Created, DueDate, Active from UserTask where Id in @range", range);
            }
        }

        public IEnumerable<UserTask> All()
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Query<UserTask>(@"Select Id, Name, Description, UserId, StatusId, PriorityId, Created, DueDate, Active from UserTask");
            }
        }
        public void Update(UserTask entity)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                connection.QueryFirstOrDefault(@"Update User set Name = @Name, Description = @Description, UserId = @Userid, StatusId = @StatusId,
                                                PriorityId = @PriorityId, DueDate = @Duedate, Active = @Active where Id = @Id",
                    new { entity.Id, entity.Name, entity.Description, entity.UserId, entity.StatusId, entity.PriorityId, entity.DueDate, entity.Active});
            }
        }

        public IEnumerable<UserTaskDto> getDTOs(int userId)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Query<UserTaskDto>(@"SELECT 
                                                        ut.Id, 
                                                        ut.Name, 
                                                        ut.Description, 
                                                        ut.UserId, 
                                                        ut.StatusId, 
                                                        ut.PriorityId, 
                                                        ut.Created, 
                                                        ut.DueDate,
                                                        ut.Active,
                                                        u.Username, 
                                                        u.Name AS User,
                                                        p.Name AS Priority,
                                                        s.Name AS Status
                                                    FROM UserTask ut
                                                    LEFT JOIN User u ON u.Id = ut.UserId
                                                    LEFT JOIN Priorities p ON p.Id = ut.PriorityId
                                                    LEFT JOIN Status s ON s.Id = ut.StatusId
                                                    WHERE ut.UserId = @userId;
                                                    ", new { userId});
            }
        }
    }
}

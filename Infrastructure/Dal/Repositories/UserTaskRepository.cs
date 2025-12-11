using Infrastructure.Dal.interfaces;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using System.Data.SQLite;
using static Dapper.SqlMapper;

namespace Infrastructure.Dal.Repositories
{
    public class UserTaskRepository : IRepository<UserTask>
    {
       
        public UserTask Add(UserTask entity)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                 int result = connection.ExecuteScalar<int>(@"Insert into UserTask(Name, Description, UserId, StatusId, PriorityId, Created, DueDate, Active)
                  values (@Name, @Description, @UserId, @StatusId, @PriorityId, @Created, @DueDate, @Active); SELECT last_insert_rowid();", 
                  new { entity.Name, entity.Description, entity.UserId, entity.StatusId, entity.PriorityId, entity.Created, entity.DueDate, entity.Active });
                entity.Id = result;
                return entity;
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
                connection.QueryFirstOrDefault(@"Update UserTask set Name = @Name, Description = @Description, UserId = @UserId, StatusId = @StatusId,
                                                PriorityId = @PriorityId, DueDate = @DueDate, Active = @Active where Id = @Id",
                    new { entity.Id, entity.Name, entity.Description, entity.UserId, entity.StatusId, entity.PriorityId, entity.DueDate, entity.Active});
            }
        }

        public IEnumerable<UserTaskDto> getDTOs(int userId = -1)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                bool hasUser = userId > 0;
                string sql = @"SELECT 
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
                            LEFT JOIN Status s ON s.Id = ut.StatusId";
                sql += hasUser ? " where ut.UserId = @userId;" : ";";
                connection.Open();
                if (hasUser) { return connection.Query<UserTaskDto>(sql, new { userId}); }

                return connection.Query<UserTaskDto>(sql);

            }
        }

        public List<DateDto> getDates()
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                var sql = "Select distinct Date(DueDate) as Date from UserTask order by DueDate";
                return connection.Query<DateDto>(sql).ToList();
            }

        }
    }
}

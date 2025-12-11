using Infrastructure.Dal.interfaces;
using Infrastructure.Entities;
using System.Data.SQLite;
using static Dapper.SqlMapper;

namespace Infrastructure.Dal.Repositories
{
    public class StatusRepository : IRepository<Status>
    {
        public Status Add(Status entity)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                int id = connection.ExecuteScalar<int>(@"Insert into Status(Name, Description, Active)
                  values (@Name, @Description, @Active); SELECT last_insert_rowid();", new { entity.Name, entity.Description, entity.Active });
                entity.Id = id;
                return entity;
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Execute(@"Delete from Status where Id = @id", new { id }) > 0;
            }
        }

        public Status Get(int id)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Status>(@"Select Id, Name, Descripton, Active from Status where Id = @id", new { id });
            }
        }


        public IEnumerable<Status> All()
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Query<Status>(@"Select Id, Name, Description, Active from Status");
            }
        }
        public void Update(Status entity)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                connection.QueryFirstOrDefault(@"Update Status set Name = @Name, Description = @Description, Active = @Active where Id = @Id",
                    new { entity.Id, entity.Name, entity.Description, entity.Active });
            }
        }
    }
}

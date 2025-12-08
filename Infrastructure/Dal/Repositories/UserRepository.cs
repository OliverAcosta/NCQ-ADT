using Infrastructure.Dal.interfaces;
using Infrastructure.Entities;
using System.Data.SQLite;
using static Dapper.SqlMapper;

namespace Infrastructure.Dal.Repositories
{
    public class UserRepository : IRepository<User>, IRangeRespository<User>
    {
       
        public void Add(User entity)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                connection.Execute(@"Insert into User(Username, Name, Email, Password, Active)
                  values (@Username, @Name, @Email, @Password, @Active)", 
                  new { entity.UserName, entity.Name,  entity.Email, entity.Password, entity.Active });
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Execute(@"Delete from User where Id = @id", new { id }) > 0;
            }
        }

        public User Get(int id)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<User>(@"Select Id, Username, Name, Email, Password, Active from User where Id = @id", new { id });
            }
        }

        public IEnumerable<User> GetRange(int[] range)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Query<User>(@"Select Id, Username, Name, Email, Password, Active from User where Id in @range", range);
            }
        }

        public IEnumerable<User> All()
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Query<User>(@"Select Id, Username, Name, Email, Password, Active from User");
            }
        }
        public void Update(User entity)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                connection.QueryFirstOrDefault(@"Update User set Username = @Username, Name = @Name, Email = @Email,
                    Password = @password, Active = @Active where Id = @Id",
                    new { entity.Id, entity.UserName, entity.Name, entity.Email, entity.Password, entity.Active });
            }
        }
    }
}

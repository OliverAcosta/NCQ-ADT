using Infrastructure.Dal.interfaces;
using Infrastructure.Entities;
using System.Data.SQLite;
using static Dapper.SqlMapper;

namespace Infrastructure.Dal.Repositories
{
    public class PrioritiesRepository : IRepository<Priorities>, IRangeRespository<Priorities>
    {
       
        public void Add(Priorities entity)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                connection.Execute(@"Insert into Priorities(Name, Description, Active)
                  values (@Name, @Description, Active)", new { entity.Name, entity.Description, entity.Active });
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Execute(@"Delete from Priorities where Id = @id", new { id }) > 0;
            }
        }

        public Priorities Get(int id)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Priorities>(@"Select Id, Name, Description, Active from Priorities where Id = @id", new { id });
            }
        }

        public IEnumerable<Priorities> GetRange(int[] range)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Query<Priorities>(@"Select Id, Name, Description, Active from Priorities where Id in @range", range);
            }
        }

        public IEnumerable<Priorities> All()
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Query<Priorities>(@"Select Id, Name, Description, Active from Priorities");
            }
        }
        public void Update(Priorities entity)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                connection.QueryFirstOrDefault(@"Update Priorities set Name = @Name, Description = @Description, Active = @Active where Id = @Id",
                    new { entity.Id, entity.Name, entity.Description, entity.Active });
            }
        }
    }
}

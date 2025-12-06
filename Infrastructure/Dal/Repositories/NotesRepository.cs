using Infrastructure.Dal.interfaces;
using Infrastructure.Entities;
using System.Data.SQLite;
using static Dapper.SqlMapper;

namespace Infrastructure.Dal.Repositories
{
    public class NotesRepository : IRepository<Notes>, IRangeRespository<Notes>
    {
       
        public void Add(Notes entity)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                connection.Execute(@"Insert into Notes(UserTaskId, Note, Active)
                  values (@UserTaskId, @Note, @Active)", new { entity.UserTaskId, entity.Note, entity.Active });
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Execute(@"Delete from Notes where Id = @id", new { id }) > 0;
            }
        }

        public Notes Get(int id)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Notes>(@"Select Id, UserTaskId, Note, Active from Notes where Id = @id", new { id });
            }
        }

        public IEnumerable<Notes> GetRange(int[] range)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Query<Notes>(@"Select Id, UserTaskId, Note, Active from Notes where Id in @range", range);
            }
        }

        public IEnumerable<Notes> All()
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Query<Notes>(@"Select Id, UserTaskId, Note, Active from Notes");
            }
        }

        public void Update(Notes entity)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                connection.QueryFirstOrDefault(@"Update set UserTaskId = @UserTaskId, Notes = @Notes where Id = @id",
                    new { entity.Id, entity.UserTaskId, entity.Note });
            }
        }
    }
}

using Infrastructure.Dal.interfaces;
using Infrastructure.Entities;
using System.Data.SQLite;
using static Dapper.SqlMapper;

namespace Infrastructure.Dal.Repositories
{
    public class NotesRepository : IRepository<Notes>, IRangeRespository<Notes>
    {
       
        public Notes Add(Notes entity)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                int id = connection.ExecuteScalar<int>(@"Insert into Notes(UserTaskId, Note, Created, Active)
                  values (@UserTaskId, @Note, @Created, @Active); SELECT last_insert_rowid();", new { entity.UserTaskId, entity.Note, entity.Created, entity.Active });
                entity.Id = id;
                return entity;
            }
        }

        public bool Delete(int userTaskId)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Execute(@"Delete from Notes where Id = @userTaskId", new { userTaskId }) > 0;
            }
        }

        public Notes Get(int id)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Notes>(@"Select Id, UserTaskId, Note, Created, Active from Notes where Id = @id", new { id });
            }
        }

        public IEnumerable<Notes> GetRange(int sometypeId)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Query<Notes>(@"Select Id, UserTaskId, Note, Created, Active from Notes where UserTaskId = @UserTaskId", new { UserTaskId = sometypeId});
            }
        }

        public IEnumerable<Notes> All()
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                return connection.Query<Notes>(@"Select Id, UserTaskId, Note, Created, Active from Notes");
            }
        }

        public void Update(Notes entity)
        {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                connection.QueryFirstOrDefault(@"Update set UserTaskId = @UserTaskId, Note = @Note, Created = @Created, Active = @Active where Id = @Id",
                    new { entity.Id, entity.UserTaskId, entity.Note, entity.Created, entity.Active });
            }
        }
    }
}

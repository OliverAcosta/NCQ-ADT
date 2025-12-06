
using Dapper;
using Infrastructure.Dal.interfaces;
using System.Data.SQLite;
using Utilities;

namespace Infrastructure.Dal.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
    {
        string connectionString = "Data Source=tasks.db;Version=3;";
        public GenericRepository()
        {

        }
        public void Add(TEntity entity)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
               string sql = QueryUtility.GetInsertString<TEntity>();
               var dic = QueryUtility.GetValues(entity); 
               connection.Open();
               connection.Execute(sql, dic);
            }
        }

        public bool Delete(int id)
        {
            string sql = "Delete from " + typeof(TEntity).Name + " where Id = @id";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                return connection.Execute(sql, new { id }) > 0;
            }
        }

        public TEntity Get(int id)
        {
            string sql = "Select from " + typeof(TEntity).Name + " where Id = @id";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault(sql, new { id });
            }
        }

        public void Update(TEntity entity)
        {
            string sql = QueryUtility.GetUpdateString<TEntity>();
            var values = QueryUtility.GetValues(entity);
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                connection.Execute(sql,values);
            }
        }
    }
}

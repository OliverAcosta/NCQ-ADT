

using System.Data.SQLite;

namespace Infrastructure.Dal
{
    public class DatabaseCreation
    {
        public void createDatabase()
        {
            if (this.checkForUser()) return;
            string[] sqlfiles = { "database/database.sql", "database/insert.sql" };
            var list = new List<string>();

            foreach (string sqlfile in sqlfiles) {
                list.Add(File.ReadAllText(sqlfile));
            }

            foreach (string l in list) {
                this.ExecuteQuery(l);
            }
        }


        private bool checkForUser()
        {
            string sql = "select id from user";
            try
            {
                using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
                {
                    connection.Open();
                    using (var cmd = new SQLiteCommand(sql, connection))
                    {
                        return (cmd.ExecuteScalar() != null);
                    }
                }
            }catch(Exception)
            {
                return false;
            }
        }
        private void ExecuteQuery(string sql) {
            using (var connection = new SQLiteConnection(DatabaseConnections.connectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

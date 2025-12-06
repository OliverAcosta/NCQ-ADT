using System.Reflection.Metadata;
using System.Text;
using Utilities.enums;

namespace Utilities
{
    public class QueryUtility
    {
        public static string GetInsertString<T>()
        {
            string query = GetOperationStr(QueryEnum.INSERT);
            var type = typeof(T);
            var sb = new StringBuilder();
            var props = type.GetProperties().Where(m => m.Name != "Id").Select(m => m.Name).ToArray(); // selecciono los nombres de las columnas;
                  
            sb.AppendFormat(query,
                type.Name, // nombre de la tabla
                string.Join(',', props), // columnas a agregar
                string.Join(',', props.Select((m) => "@" + m)) // parametros a agregar
            );
            return sb.ToString();
        }


        public static Dictionary<string, object> GetValues<T>(T entity)
        {
            string query = GetOperationStr(QueryEnum.INSERT);
            var props = typeof(T).GetProperties().Where(m=> m.Name != "Id");
            var values = new Dictionary<string, object>();
            foreach (var item in props)
            {
                values.Add(item.Name, item.GetValue(entity));
            }
            return values;
        }


        public static string GetUpdateString<T>()
        {
            string query = GetOperationStr(QueryEnum.UPDATE);
            var type = typeof(T);
            var sb = new StringBuilder();
            var props = type.GetProperties().Where(m => m.Name != "Id").Select(m => m.Name).ToArray(); // selecciono los nombres de las columnas;

            sb.AppendFormat(query,
                type.Name, // nombre de la tabla
                string.Join(',', props.Select((m) => string.Format("{0} = {1}", m, '@' + m))) // parametros a actualizar
            );
            return sb.ToString();
        }

        public static string GetUpdateString<T>(IEnumerable<string> names, string whereclause)
        {
            string query = GetOperationStr(QueryEnum.UPDATE);
            var type = typeof(T);
            var sb = new StringBuilder();
            //var props = type.GetProperties().Where(m => m.Name != "Id").Select(m => m.Name).ToArray(); // selecciono los nombres de las columnas;

            sb.AppendFormat(query,
                type.Name, // nombre de la tabla
                string.Join(',', names.Select((m) => string.Format("{0} = {1}", m, '@'+ m))), // parametros a actualizar
                 whereclause
            );
            return sb.ToString();
        }

        private static string GetOperationStr(QueryEnum qe)
        {
            string query = string.Empty;
            switch (qe)
            {
                case QueryEnum.INSERT:
                    query = "Insert into {0}({1}) values({2})";
                    break;
                case QueryEnum.UPDATE:
                    query = "update {0} set {1} where {2}";
                    break;
                case QueryEnum.DELETE:
                    query = "delete from {0} where {1}";
                    break;
            }
            return query;
        }
    }
}


using System.Text;

namespace Utilities
{
    public class ReflexitionUtility
    {
        public static void copyDataOnEntity<T>(T entity, Dictionary<string, object> values)
        {
            var props = entity.GetType().GetProperties();

            foreach (var item in values)
            {
                var prop = props.FirstOrDefault(m => m.Name.Equals(item.Key, StringComparison.InvariantCultureIgnoreCase));
                if (prop != null)
                {

                    prop.SetValue(entity, Convert.ChangeType(item.Value.ToString(), prop.PropertyType));
                }
            }

        }

    }
}

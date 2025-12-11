
namespace Infrastructure.Dal.interfaces
{
    public interface IRepository<T>
    {
       T Get(int id);
       T Add(T entity); 
       void Update(T entity); 
       bool Delete(int id); 
    }

}

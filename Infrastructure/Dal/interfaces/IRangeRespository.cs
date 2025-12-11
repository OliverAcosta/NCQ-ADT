

namespace Infrastructure.Dal.interfaces
{
    public interface IRangeRespository<T>
    {
        public IEnumerable<T> GetRange(int someTypeId);

        public IEnumerable<T> All();

    }
}



namespace Infrastructure.Dal.interfaces
{
    public interface IRangeRespository<T>
    {
        public IEnumerable<T> GetRange(int[] range);

        public IEnumerable<T> All();

    }
}

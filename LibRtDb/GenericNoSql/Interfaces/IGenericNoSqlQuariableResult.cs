using System.Threading.Tasks;

namespace LibRtDb.GenericNoSql.Interfaces
{
    public interface IGenericNoSqlQuariableResult<T>
    {
        public T First();
        public Task<T> FirstAsync();

        public T FirstOrDefault();
        public Task<T> FirstOrDefaultAsync();

        /// <summary>
        /// Returns the only document of resultset, and throw an exception if there not exactly one document in the sequence
        /// </summary>
        public T Single();
        public Task<T> SingleAsync();

        /// <summary>
        /// Returns the only document of resultset, or null if resultset are empty; this method throw an exception if there not exactly one document in the sequence
        /// </summary>
        public T SingleOrDefault();
        public Task<T> SingleOrDefaultAsync();

    }
}

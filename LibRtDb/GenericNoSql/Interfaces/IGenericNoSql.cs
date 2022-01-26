using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibRtDb.GenericNoSql.Interfaces
{
    public interface IGenericNoSql : IDisposable
    {

        /// <summary>
        /// Avoid Dipose() it in wrong moments in case of async usage
        /// </summary>
        /// <returns></returns>
        public IGenericNoSqlQuerySession QuerySession();

        /// <summary>
        /// Avoid Dipose() it in wrong moments in case of async usage
        /// </summary>
        /// <returns></returns>
        public IGenericNoSqlLightweightSession LightweightSession();

        public void TruncateCollection<T>();
        public Task TruncateCollectionAsync<T>();

        public void BulkInsert<T>(IReadOnlyCollection<T> Documents, int BatchSize = 100);
        public Task BulkInsertAsync<T>(IReadOnlyCollection<T> Documents, int BatchSize = 100);

    }
}

using LibRtDb.DTO.DeviceConfigs;
using LibRtDb.DTO.DynamicKeys;
using LibRtDb.DTO.Events;
using LibRtDb.DTO.Languages;
using LibRtDb.DTO.SerialNumbers;
using LibRtDb.GenericNoSql.Implementations.LiteDB;
using LibRtDb.GenericNoSql.Interfaces;
using Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibRtDb.GenericNoSql.Implementations.MartenDB
{
    public class MartenDB : IGenericNoSql
    {

        private DocumentStore Context { get; set; }

        public MartenDB(DocumentStore Context)
        {
            this.Context = Context;
        }

        public void TruncateCollection<T>()
        {
            Context.Advanced.Clean.DeleteDocumentsFor(typeof(T));
        }

        public Task TruncateCollectionAsync<T>()
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => TruncateCollection<T>());
        }

        public void BulkInsert<T>(IReadOnlyCollection<T> Documents, int BatchSize = 100)
        {
            Context.BulkInsert<T>(Documents, BulkInsertMode.InsertsOnly, BatchSize);
        }

        public Task BulkInsertAsync<T>(IReadOnlyCollection<T> Documents, int BatchSize = 100)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => BulkInsert(Documents, BatchSize));
        }

        public IGenericNoSqlLightweightSession LightweightSession()
        {
            return new MartenDBLightweightSession(Context.LightweightSession());
        }

        public IGenericNoSqlQuerySession QuerySession()
        {
            return new MartenDBQuerySession(Context.QuerySession());
        }

        public void Dispose()
        {
            if (GenericAsyncEngine.Instance != null)
            {
                GenericAsyncEngine.Instance.Dispose();
            }
        }
    }
}

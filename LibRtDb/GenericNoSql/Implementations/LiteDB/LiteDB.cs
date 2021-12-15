using LibRtDb.GenericNoSql.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibRtDb.GenericNoSql.Implementations.LiteDB
{
    public class LiteDB : IGenericNoSql
    {

        private ILiteDatabase Context { get; set; }
        
        public LiteDB(ILiteDatabase Context)
        {
            this.Context = Context;
        }

        public void TruncateCollection<T>()
        {
            var collectionName = typeof(T).Name;
            Context.DropCollection(collectionName);
        }

        public Task TruncateCollectionAsync<T>()
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => TruncateCollection<T>());
        }

        public void BulkInsert<T>(IReadOnlyCollection<T> Documents, int BatchSize = 100)
        {
            Context.GetCollection<T>().InsertBulk(Documents, BatchSize);
        }

        public Task BulkInsertAsync<T>(IReadOnlyCollection<T> Documents, int BatchSize = 100)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => BulkInsert(Documents, BatchSize));
        }

        public IGenericNoSqlQuerySession QuerySession()
        {
            return new LiteDbQuerySession(Context);
        }

        public IGenericNoSqlLightweightSession LightweightSession()
        {
            return new LiteDbLightweightSession(Context);
        }

        public void Dispose()
        {
            if(GenericAsyncEngine.Instance != null)
            {
                GenericAsyncEngine.Instance.Dispose();
            }
        }
    }
}

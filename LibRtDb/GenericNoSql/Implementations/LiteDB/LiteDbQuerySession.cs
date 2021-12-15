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
    public class LiteDbQuerySession : IGenericNoSqlQuerySession
    {

        private ILiteDatabase _querySession;

        public LiteDbQuerySession(ILiteDatabase QuerySession)
        {
            _querySession = QuerySession;
        }

        public void Dispose()
        {
            _querySession.Dispose();
        }

        public IGenericNoSqlQuariable<T> Query<T>()
        {
            return new LiteDbQuariable<T>(_querySession.GetCollection<T>());
        }
    }
}

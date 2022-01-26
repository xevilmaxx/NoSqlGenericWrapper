using LibRtDb.GenericNoSql.Interfaces;
using LiteDB;

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

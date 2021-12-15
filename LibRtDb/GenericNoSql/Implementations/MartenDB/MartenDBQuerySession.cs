using LibRtDb.GenericNoSql.Interfaces;
using Marten;

namespace LibRtDb.GenericNoSql.Implementations.MartenDB
{
    public class MartenDBQuerySession : IGenericNoSqlQuerySession
    {

        private IQuerySession _querySession;

        public MartenDBQuerySession(IQuerySession QuerySession)
        {
            _querySession = QuerySession;
        }

        public void Dispose()
        {
            _querySession.Dispose();
        }

        public IGenericNoSqlQuariable<T> Query<T>()
        {
            return new MartenDbQuariable<T>(_querySession);
        }

    }
}

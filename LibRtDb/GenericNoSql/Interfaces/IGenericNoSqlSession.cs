using System;

namespace LibRtDb.GenericNoSql.Interfaces
{
    public interface IGenericNoSqlSession : IDisposable
    {

        public IGenericNoSqlQuariable<T> Query<T>();

    }
}

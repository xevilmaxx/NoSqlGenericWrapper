using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibRtDb.GenericNoSql.Interfaces
{
    public interface IGenericNoSqlQuerySession : IGenericNoSqlSession
    {

        //public IGenericNoSqlQuariable<T> Query<T>(Expression<Func<T, bool>> predicate);

    }
}

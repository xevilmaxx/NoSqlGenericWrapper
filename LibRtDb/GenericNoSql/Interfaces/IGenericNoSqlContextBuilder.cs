using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibRtDb.GenericNoSql.Interfaces
{
    public interface IGenericNoSqlContextBuilder
    {

        /// <summary>
        /// Add Custom Connection String
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public IGenericNoSqlContextBuilder SetConnectionString(string ConnectionString);

        /// <summary>
        /// Add Custom DatabaseName
        /// </summary>
        /// <param name="DbName"></param>
        /// <returns></returns>
        public IGenericNoSqlContextBuilder SetDatabasebName(string DbName);

        /// <summary>
        /// Last method to Invoke in order to generate the Context
        /// </summary>
        /// <returns></returns>
        public IGenericNoSql GenerateContext();

    }
}

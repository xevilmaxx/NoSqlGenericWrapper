using LibRtDb.GenericNoSql.Interfaces;
using LiteDB;
using System.Text.RegularExpressions;

namespace LibRtDb.GenericNoSql.Implementations.LiteDB
{
    public class LiteDBContextBuilder : IGenericNoSqlContextBuilder
    {

        private string ConnString { get; set; }
        private string DbName { get; set; } = "parko_rts";

        private Regex DbNameRegex = new Regex(@"\\.+.db;", RegexOptions.Compiled);

        public IGenericNoSql GenerateContext()
        {

            //var xx = DbNameRegex.Match(ConnString).Value;

            if (!string.IsNullOrEmpty(DbName))
            {
                //replace DbName with supplied one
                ConnString = DbNameRegex.Replace(ConnString, $"\\{DbName}.db;");
            }

            var context = new LiteDatabase(ConnString);
            return new LiteDB(context);

        }

        public IGenericNoSqlContextBuilder SetDatabasebName(string DatabaseName)
        {
            DbName = DatabaseName;
            return this;
        }

        public IGenericNoSqlContextBuilder SetConnectionString(string ConnectionString)
        {
            ConnString = ConnectionString;
            return this;
        }

    }
}

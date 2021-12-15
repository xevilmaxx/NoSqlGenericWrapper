# NoSqlGenericWrapper
Marten + LiteDb wrapped together

Features:
* Primitive Async Engine in order to support Asyn for LiteDb and Marten Linq Query
* LiteDb
* Marten3
* Most of basic CRUD operations and queries


Usage:


            var ContextLiteDb = new LibRtDb.GenericNoSql.Implementations.LiteDB.LiteDBContextBuilder()
                .SetConnectionString(ConnectionStringLiteDb)
                .SetDatabasebName(DatabaseName)
                .GenerateContext();

            var ContextMarten = new LibRtDb.GenericNoSql.Implementations.MartenDB.MartenDBContextBuilder()
                .SetConnectionString(ConnectionStringMarten)
                .SetDatabasebName(DatabaseName)
                .GenerateContext();

            using (var db = ContextMarten.QuerySession())
            {
                var result = db.Query<JsonDeviceConfigs>().Where(x => x.Id == 1).ToList();
            }
            
            using (var db = ContextLiteDb.LightweightSession())
            {
                db.Insert<JsonDeviceConfigs>(cfg);
                db.SaveChanges();
            }

Factory Usage with configs in AppSettings.json:

            using (var db = DbFactory.GetContext().QuerySession())
            {
            ....
            }

And here is AppSetting.json sample:

            {
              "DbType": 1,
              "ConnectionStrings": {
                "Postgres": "host=127.0.0.1;database=postgres;password=test;username=postgres;",
                "LiteDb": "Filename=..\\test.db;password=pwd;connection=shared"
              }
            }
            
 You can also change code a bit in order to supply DbName in settings

# NoSqlGenericWrapper
Marten + LiteDb wrapped together

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

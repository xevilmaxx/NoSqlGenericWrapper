using Google.Protobuf.Collections;
using LibRtDb.DTO.DeviceConfigs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestLibRtDb
{
    public class GenericDbTest
    {

        private string ConnectionStringMarten { get; set; }
        private string ConnectionStringLiteDb { get; set; }
        private string DatabaseName { get; set; }

        [SetUp]
        public void Setup()
        {

            DatabaseName = "parko_rts";
            ConnectionStringLiteDb = "Filename=.\\parko.db;password=pwd;connection=shared";
            ConnectionStringMarten = "host=127.0.0.1;database=postgres;password=pwd;username=postgres;";

        }

        [Test]
        public void CheckConnectionMarten()
        {

            var ContextMarten = new LibRtDb.GenericNoSql.Implementations.MartenDB.MartenDBContextBuilder()
                .SetConnectionString(ConnectionStringMarten)
                .SetDatabasebName(DatabaseName)
                .GenerateContext();

            using (var db = ContextMarten.QuerySession())
            {
                var result = db.Query<JsonDeviceConfigs>().Where(x => x.Id == 1).ToList();
            }

            Assert.Pass("Marten connection is ok");

        }

        [Test]
        public void CheckConnectionLiteDb()
        {

            var ContextLiteDb = new LibRtDb.GenericNoSql.Implementations.LiteDB.LiteDBContextBuilder()
                .SetConnectionString(ConnectionStringLiteDb)
                .SetDatabasebName(DatabaseName)
                .GenerateContext();

            using (var db = ContextLiteDb.QuerySession())
            {
                var result = db.Query<JsonDeviceConfigs>().Where(x => x.Id == 1).ToList();
            }

            Assert.Pass("LiteDb connection is ok");

        }

        [Test]
        public void CheckInsertLiteDb()
        {

            var ContextLiteDb = new LibRtDb.GenericNoSql.Implementations.LiteDB.LiteDBContextBuilder()
                .SetConnectionString(ConnectionStringLiteDb)
                .SetDatabasebName(DatabaseName)
                .GenerateContext();

            var cfg = new JsonDeviceConfigs()
            {
                Name = "1",
                Description = "1",
                DeviceType = 2,
                Configs = new System.Collections.Generic.List<DevConfig>()
                {
                    new DevConfig()
                    {
                        Key = "Hello",
                        Value = "World",
                        Description = "Test"
                    }
                }
            };

            using (var db = ContextLiteDb.LightweightSession())
            {
                db.InsertAsync<JsonDeviceConfigs>(cfg);
                var result = db.Query<JsonDeviceConfigs>().FirstOrDefault();
            }

            Assert.Pass("LiteDb connection is ok");

        }

        [Test]
        public async Task CheckAsyncLiteDbAsync()
        {

            var ContextLiteDb = new LibRtDb.GenericNoSql.Implementations.LiteDB.LiteDBContextBuilder()
                .SetConnectionString(ConnectionStringLiteDb)
                .SetDatabasebName(DatabaseName)
                .GenerateContext();


            //var xcv = ContextLiteDb.TruncateCollectionAsync<JsonDeviceConfigs>();

            var cfg = new JsonDeviceConfigs()
            {
                Name = "15496",
                Description = "1",
                DeviceType = 2,
                Configs = new System.Collections.Generic.List<DevConfig>()
                {
                    new DevConfig()
                    {
                        Key = "Hello",
                        Value = "World",
                        Description = "Test"
                    }
                }
            };

            JsonDeviceConfigs res = null;
            using (var db = ContextLiteDb.LightweightSession())
            {
                await db.InsertAsync(cfg);
                res = await db.Query<JsonDeviceConfigs>().FirstOrDefaultAsync();
                await db.SaveChangesAsync();
            }

            Assert.AreEqual(res.Name, cfg.Name);

            Assert.Pass("LiteDb connection is ok");

        }

        [Test]
        public void CheckRepeatedField()
        {

            //var ContextLiteDb = new LibRtDb.GenericNoSql.Implementations.MartenDB.MartenDBContextBuilder()
            //    .SetConnectionString(ConnectionStringMarten)
            //    .SetDatabasebName(DatabaseName)
            //    .GenerateContext();

            var ContextLiteDb = new LibRtDb.GenericNoSql.Implementations.LiteDB.LiteDBContextBuilder()
                .SetConnectionString(ConnectionStringLiteDb)
                .SetDatabasebName(DatabaseName)
                .GenerateContext();


            //var xcv = ContextLiteDb.TruncateCollectionAsync<JsonDeviceConfigs>();

            var cfg2 = new JsonDeviceConfigs()
            {
                Name = "15496",
                Description = "1",
                DeviceType = 2,
                Configs = new System.Collections.Generic.List<DevConfig>()
                {
                    new DevConfig()
                    {
                        Key = "Hello",
                        Value = "World",
                        Description = "Test"
                    }
                }
            };

            var cfg = new RepeatedField<string>()
            {
                "e",
                "t"
            };

            JsonDeviceConfigs res = null;
            using (var db = ContextLiteDb.LightweightSession())
            {
                db.Upsert(new List<JsonDeviceConfigs>() { cfg2 });
                //db.Upsert<IEnumerable<string>>(cfg.AsEnumerable());
                //db.Upsert(cfg.AsEnumerable());
                db.SaveChanges();
            }

            //Assert.AreEqual(res.Name, cfg.Name);

            Assert.Pass("MartenDB nested is ok");

        }

        [Test]
        public void CollectionRedirectionCachingTest()
        {

            //var ContextLiteDb = new LibRtDb.GenericNoSql.Implementations.MartenDB.MartenDBContextBuilder()
            //    .SetConnectionString(ConnectionStringMarten)
            //    .SetDatabasebName(DatabaseName)
            //    .GenerateContext();

            var ContextLiteDb = new LibRtDb.GenericNoSql.Implementations.LiteDB.LiteDBContextBuilder()
                .SetConnectionString(ConnectionStringLiteDb)
                .SetDatabasebName(DatabaseName)
                .GenerateContext();


            //var xcv = ContextLiteDb.TruncateCollectionAsync<JsonDeviceConfigs>();

            var cfg2 = new JsonDeviceConfigs()
            {
                Name = "15496",
                Description = "1",
                DeviceType = 2,
                Configs = new System.Collections.Generic.List<DevConfig>()
                {
                    new DevConfig()
                    {
                        Key = "Hello",
                        Value = "World",
                        Description = "Test"
                    }
                }
            };

            var cfg3 = new JsonDeviceConfigs()
            {
                Name = "77777",
                Description = "1",
                DeviceType = 2,
                Configs = new System.Collections.Generic.List<DevConfig>()
                {
                    new DevConfig()
                    {
                        Key = "Hello",
                        Value = "World",
                        Description = "Test"
                    }
                }
            };

            JsonDeviceConfigs res = null;
            using (var db = ContextLiteDb.LightweightSession())
            {
                db.Upsert(new List<JsonDeviceConfigs>() { cfg2 });
                db.Upsert(new List<JsonDeviceConfigs>() { cfg3 });
                db.SaveChanges();
            }

            //Assert.AreEqual(res.Name, cfg.Name);

            Assert.Pass("MartenDB nested is ok");

        }

    }
}
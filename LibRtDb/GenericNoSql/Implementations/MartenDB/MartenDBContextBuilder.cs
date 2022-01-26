using LibRtDb.DTO.DeviceConfigs;
using LibRtDb.DTO.DynamicKeys;
using LibRtDb.DTO.Events;
using LibRtDb.DTO.Languages;
using LibRtDb.DTO.SerialNumbers;
using LibRtDb.GenericNoSql.Interfaces;
using Marten;

namespace LibRtDb.GenericNoSql.Implementations.MartenDB
{
    public class MartenDBContextBuilder : IGenericNoSqlContextBuilder
    {

        private string DbName { get; set; } = "parko_rts";
        private string ConnectionString { get; set; }

        public IGenericNoSql GenerateContext()
        {
            var Context = DocumentStore.For(_ =>
            {

                //Custom connection string
                _.Connection(ConnectionString);

                _.NameDataLength = 250;

                //Custom Schema Name
                _.DatabaseSchemaName = DbName;

                //add json indexes also
                // Add a gin index to Company's json data storage
                //may be commented if performance is poor and you dont need do much readings
                //_.Schema.For<Transit>().GinIndexJsonData();
                //_.Schema.For<User>().GinIndexJsonData();
                _.Schema.For<JsonDeviceConfigs>().GinIndexJsonData();
                _.Schema.For<DynamicKey>().GinIndexJsonData();
                _.Schema.For<Event>().GinIndexJsonData();
                _.Schema.For<LanguageResource>().GinIndexJsonData();
                _.Schema.For<SerialNumber>().GinIndexJsonData();

                //My custom override for comparing objects not supported by Marten natively (for now alpha-8)
                //Doable only in Marten >= v4
                //_.Linq.MethodCallParsers.Add(new LinqEqualsExt());

            });

            return new MartenDB(Context);

        }

        public IGenericNoSqlContextBuilder SetConnectionString(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
            return this;
        }

        public IGenericNoSqlContextBuilder SetDatabasebName(string DbName)
        {
            this.DbName = DbName;
            return this;
        }

    }
}

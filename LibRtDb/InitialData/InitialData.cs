using Marten;
using Marten.Schema;

namespace LibRtDb.InitialData
{
    /// <summary>
    /// Generic Class needed to Upsert Initial data by Marten Specifications:
    /// https://martendb.io/documentation/documents/basics/initial_data/
    /// </summary>
    public class InitialData : IInitialData
    {
        private readonly object[] _initialData;

        public InitialData(params object[] initialData)
        {
            _initialData = initialData;
        }

        public void Populate(IDocumentStore store)
        {
            using (var session = store.LightweightSession())
            {
                // Marten UPSERT will cater for existing records
                session.Store(_initialData);
                session.SaveChanges();
            }
            //return Task.CompletedTask;
        }
    }
}

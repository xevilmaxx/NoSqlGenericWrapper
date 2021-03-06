using Google.Protobuf.Collections;
using LibRtDb.CustomExceptions;
using LibRtDb.GenericNoSql.Interfaces;
using LiteDB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace LibRtDb.GenericNoSql.Implementations.LiteDB
{
    public class LiteDbLightweightSession : IGenericNoSqlLightweightSession
    {

        private ILiteDatabase DocumentSession;

        //Tuple<Document Type, MethodName (on which were executed previously), MethodInfo is already previously generated generic method
        private Dictionary<Tuple<Type, string>, MethodInfo> CachedMethodsRedirection;

        public LiteDbLightweightSession(ILiteDatabase QuerySession)
        {
            DocumentSession = QuerySession;
            CachedMethodsRedirection = new Dictionary<Tuple<Type, string>, MethodInfo>();
        }

        public void Dispose()
        {
            DocumentSession.Dispose();
        }


        public void Insert<T>(T Document)
        {
            //Valid for Lists and Repeated Fields
            if (Document is IEnumerable)
            {

                MethodInfo localMethod;
                var tuple = Tuple.Create(Document.GetType(), nameof(Insert));
                if (CachedMethodsRedirection.ContainsKey(tuple) == true)
                {
                    localMethod = CachedMethodsRedirection[tuple];
                }
                else
                {
                    localMethod = GenericReflectionHelper.GetAppropriateCollectionGenericMethod(this, Document, nameof(Insert));
                    CachedMethodsRedirection.Add(tuple, localMethod);
                }

                //we are relying on implicit casting
                localMethod.Invoke(this, new object[] { Document });

            }
            else
            {

                DocumentSession.GetCollection<T>().Insert(Document);

            }
        }
        public Task InsertAsync<T>(T Document)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => Insert(Document));
        }

        public void Insert<T>(IEnumerable<T> Document)
        {
            DocumentSession.GetCollection<T>().Insert(Document);
        }
        public Task InsertAsync<T>(IEnumerable<T> Document)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => Insert(Document));
        }

        public void Insert<T>(RepeatedField<T> Document)
        {
            Insert(Document.AsEnumerable());
        }
        public Task InsertAsync<T>(RepeatedField<T> Document)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => Insert(Document.AsEnumerable()));
        }

        public void Update<T>(T Document)
        {
            //Valid for Lists and Repeated Fields
            if (Document is IEnumerable)
            {

                MethodInfo localMethod;
                var tuple = Tuple.Create(Document.GetType(), nameof(Update));
                if (CachedMethodsRedirection.ContainsKey(tuple) == true)
                {
                    localMethod = CachedMethodsRedirection[tuple];
                }
                else
                {
                    localMethod = GenericReflectionHelper.GetAppropriateCollectionGenericMethod(this, Document, nameof(Update));
                    CachedMethodsRedirection.Add(tuple, localMethod);
                }

                //we are relying on implicit casting
                localMethod.Invoke(this, new object[] { Document });

            }
            else
            {

                DocumentSession.GetCollection<T>().Update(Document);

            }
        }
        public Task UpdateAsync<T>(T Document)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => Update(Document));
        }

        public void Update<T>(IEnumerable<T> Document)
        {
            DocumentSession.GetCollection<T>().Update(Document);
        }
        public Task UpdateAsync<T>(IEnumerable<T> Document)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => Update(Document));
        }

        public void Update<T>(RepeatedField<T> Document)
        {
            Update(Document.AsEnumerable());
        }
        public Task UpdateAsync<T>(RepeatedField<T> Document)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => Update(Document.AsEnumerable()));
        }


        public void Upsert<T>(T Document)
        {
            //Valid for Lists and Repeated Fields
            if (Document is IEnumerable)
            {

                //var enumedResult = GenericReflectionHelper.CastToEnumerable(Document);
                //var collection = GenericReflectionHelper.GetDynamicCollection(_documentSession, Document);

                //foreach (dynamic enumed in enumedResult)
                //{
                //    collection.Upsert(enumed);
                //}

                MethodInfo localMethod;
                var tuple = Tuple.Create(Document.GetType(), nameof(Upsert));
                if (CachedMethodsRedirection.ContainsKey(tuple) == true)
                {
                    localMethod = CachedMethodsRedirection[tuple];
                }
                else
                {
                    localMethod = GenericReflectionHelper.GetAppropriateCollectionGenericMethod(this, Document, nameof(Upsert));
                    CachedMethodsRedirection.Add(tuple, localMethod);
                }

                //we are relying on implicit casting
                localMethod.Invoke(this, new object[] { Document });


            }
            else
            {

                DocumentSession.GetCollection<T>().Upsert(Document);

            }
        }
        public Task UpsertAsync<T>(T Document)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => Upsert(Document));
        }

        public void Upsert<T>(IEnumerable<T> Document)
        {
            DocumentSession.GetCollection<T>().Upsert(Document);
        }
        public Task UpsertAsync<T>(IEnumerable<T> Document)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => Upsert(Document));
        }

        public void Upsert<T>(RepeatedField<T> Document)
        {
            Upsert(Document.AsEnumerable());
        }
        public Task UpsertAsync<T>(RepeatedField<T> Document)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => Upsert(Document.AsEnumerable()));
        }

        /// <summary>
        /// Will attempt to recover Id value from some basic field names
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Document"></param>
        /// <returns></returns>
        /// <exception cref="CustomNoSqlDbException"></exception>
        private object GetDocumentIdFieldVal<T>(T Document)
        {            
            //The default Grpc Standard, so we mostly will get it like that,
            //those are also default Id names supported by Marten
            var propertyInfo = Document.GetType().GetProperty("Id");

            if(propertyInfo == null)
            {
                //not found field Id lets search for other possible matches
                propertyInfo = Document.GetType().GetProperty("id");
                if (propertyInfo == null)
                {
                    //not found field Id lets search for other possible matches
                    propertyInfo = Document.GetType().GetProperty("ID");
                }
            }

            //if is true, means we found our property
            if (propertyInfo != null)
            {
                //so finally get it's value
                return propertyInfo?.GetValue(Document);
            }
            else
            {
                throw new CustomNoSqlDbException("No (Id | id | ID) field found in the object");
            }
        }

        public void Delete<T>(T Document)
        {
            //Valid for Lists and Repeated Fields
            if (Document is IEnumerable)
            {

                MethodInfo localMethod;
                var tuple = Tuple.Create(Document.GetType(), nameof(Delete));
                if (CachedMethodsRedirection.ContainsKey(tuple) == true)
                {
                    localMethod = CachedMethodsRedirection[tuple];
                }
                else
                {
                    localMethod = GenericReflectionHelper.GetAppropriateCollectionGenericMethod(this, Document, nameof(Delete));
                    CachedMethodsRedirection.Add(tuple, localMethod);
                }

                //we are relying on implicit casting
                localMethod.Invoke(this, new object[] { Document });

            }
            else
            {

                //int he end of the day, all single or in memory deletions will pass through here
                var id = GetDocumentIdFieldVal(Document);
                DocumentSession.GetCollection<T>().Delete(new BsonValue(id));

            }
        }
        public Task DeleteAsync<T>(T Document)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => Delete(Document));
        }

        public void Delete<T>(IEnumerable<T> Document)
        {
            foreach (var doc in Document)
            {
                Delete(doc);
            }
        }
        public Task DeleteAsync<T>(IEnumerable<T> Document)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => Delete(Document));
        }

        public void Delete<T>(RepeatedField<T> Document)
        {
            Delete(Document.AsEnumerable());
        }
        public Task DeleteAsync<T>(RepeatedField<T> Document)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => Delete(Document.AsEnumerable()));
        }

        public void DeleteWhere<T>(Expression<Func<T, bool>> Expression)
        {
            DocumentSession.GetCollection<T>().DeleteMany(Expression);
        }
        public Task DeleteWhereAsync<T>(Expression<Func<T, bool>> Expression)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => DeleteWhere(Expression));
        }

        public IGenericNoSqlQuariable<T> Query<T>()
        {
            return new LiteDbQuariable<T>(DocumentSession.GetCollection<T>());
        }


        public void SaveChanges()
        {
            //no need for LiteDB
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
            //return GenericAsyncEngine.Instance.EnqueueAsync(() => SaveChanges());
        }

    }
}

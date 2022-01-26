using LibRtDb.GenericNoSql.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LibRtDb.GenericNoSql.Implementations.LiteDB
{
    public class LiteDbQuariable<T> : IGenericNoSqlQuariable<T>
    {

        private ILiteQueryable<T> query;

        public LiteDbQuariable(ILiteCollection<T> Collection)
        {
            query = Collection.Query();
        }

        public IGenericNoSqlQuariable<T> OrderBy<K>(Expression<Func<T, K>> keySelector)
        {
            query = query.OrderBy(keySelector);
            return this;
        }

        public IGenericNoSqlQuariable<T> OrderByDescending<K>(Expression<Func<T, K>> keySelector)
        {
            query = query.OrderByDescending(keySelector);
            return this;
        }

        public IGenericNoSqlQuariable<T> Where(Expression<Func<T, bool>> predicate)
        {
            query = query.Where(predicate);
            return this;
        }

        public IEnumerable<K> Select<K>(Expression<Func<T, K>> selector)
        {
            return query.Select(selector).ToEnumerable();
        }

        public Task<IEnumerable<K>> SelectAsync<K>(Expression<Func<T, K>> selector)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => Select(selector));
        }

        public IEnumerable<K> SelectMany<K>(Func<T, IEnumerable<K>> selector)
        {
            //just becouse idk how implement it corectly by myself, will use native linq
            return query.ToEnumerable().SelectMany(selector);
        }

        public Task<IEnumerable<K>> SelectManyAsync<K>(Func<T, IEnumerable<K>> selector)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => SelectMany<K>(selector));
        }

        public IEnumerable<TResult> SelectMany<TCollection, TResult>(Func<T, IEnumerable<TCollection>> collectionSelector, Func<T, TCollection, TResult> resultSelector)
        {
            //just becouse idk how implement it corectly by myself, will use native linq
            return query.ToEnumerable().SelectMany(collectionSelector, resultSelector);
        }

        public Task<IEnumerable<TResult>> SelectManyAsync<TCollection, TResult>(Func<T, IEnumerable<TCollection>> collectionSelector, Func<T, TCollection, TResult> resultSelector)
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => SelectMany(collectionSelector, resultSelector));
        }


        public T[] ToArray()
        {
            return query.ToArray();
        }

        public Task<T[]> ToArrayAsync()
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => ToArray());
        }

        public IEnumerable<T> ToEnumerable()
        {
            return query.ToEnumerable();
        }

        public Task<IEnumerable<T>> ToEnumerableAsync()
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => ToEnumerable());
        }

        public List<T> ToList()
        {
            return query.ToList();
        }

        public Task<List<T>> ToListAsync()
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => ToList());
        }

        public T First()
        {
            T res = query.First();
            //db.Dispose();
            return res;
        }

        public Task<T> FirstAsync()
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => First());
        }

        public T FirstOrDefault()
        {
            T res = query.FirstOrDefault();
            //db.Dispose();
            return res;
        }

        public Task<T> FirstOrDefaultAsync()
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => FirstOrDefault());
        }

        public T Single()
        {
            return query.Single();
        }

        public Task<T> SingleAsync()
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => Single());
        }

        public T SingleOrDefault()
        {
            return query.SingleOrDefault();
        }

        public Task<T> SingleOrDefaultAsync()
        {
            return GenericAsyncEngine.Instance.EnqueueAsync(() => SingleOrDefault());
        }

    }
}

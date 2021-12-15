using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibRtDb.GenericNoSql.Interfaces
{
    public interface IGenericNoSqlQuariable<T> : IGenericNoSqlQuariableResult<T>
    {

        /// <summary>
        /// Filter out some results
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IGenericNoSqlQuariable<T> Where(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Sort the documents of resultset in ascending (or descending) order according to a key (support only one OrderBy)
        /// </summary>
        public IGenericNoSqlQuariable<T> OrderBy<K>(Expression<Func<T, K>> keySelector);

        /// <summary>
        /// Sort the documents of resultset in descending order according to a key (support only one OrderBy)
        /// </summary>
        public IGenericNoSqlQuariable<T> OrderByDescending<K>(Expression<Func<T, K>> keySelector);

        /// <summary>
        /// Project each document of resultset into a new document/value based on selector expression
        /// </summary>
        public IEnumerable<K> Select<K>(Expression<Func<T, K>> selector);
        public Task<IEnumerable<K>> SelectAsync<K>(Expression<Func<T, K>> selector);

        public IEnumerable<K> SelectMany<K>(Func<T, IEnumerable<K>> selector);
        public Task<IEnumerable<K>> SelectManyAsync<K>(Func<T, IEnumerable<K>> selector);
        
        public IEnumerable<TResult> SelectMany<TCollection, TResult>(Func<T, IEnumerable<TCollection>> collectionSelector, Func<T, TCollection, TResult> resultSelector);
        public Task<IEnumerable<TResult>> SelectManyAsync<TCollection, TResult>(Func<T, IEnumerable<TCollection>> collectionSelector, Func<T, TCollection, TResult> resultSelector);

        /// <summary>
        /// Execute query and return resultset as IEnumerable of T. If T is a ValueType or String, return values only (not documents)
        /// </summary>
        public IEnumerable<T> ToEnumerable();
        public Task<IEnumerable<T>> ToEnumerableAsync();

        /// <summary>
        /// Execute query and return results as a List
        /// </summary>
        public List<T> ToList();
        public Task<List<T>> ToListAsync();

        /// <summary>
        /// Execute query and return results as an Array
        /// </summary>
        public T[] ToArray();
        public Task<T[]> ToArrayAsync();

    }
}

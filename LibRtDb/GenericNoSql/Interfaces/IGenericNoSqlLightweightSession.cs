using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibRtDb.GenericNoSql.Interfaces
{
    public interface IGenericNoSqlLightweightSession : IGenericNoSqlSession
    {

        /// <summary>
        /// Less performant if you pass collections, use dedicated overloads instead if you need performance
        /// <para/>
        /// Otherwise automatic redirection will be attempted
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Document"></param>
        public void Insert<T>(T Document);
        /// <summary>
        /// Less performant if you pass collections, use dedicated overloads instead if you need performance
        /// <para/>
        /// Otherwise automatic redirection will be attempted
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Document"></param>
        public Task InsertAsync<T>(T Document);
        public void Insert<T>(IEnumerable<T> Document);
        public Task InsertAsync<T>(IEnumerable<T> Document);
        public void Insert<T>(RepeatedField<T> Document);
        public Task InsertAsync<T>(RepeatedField<T> Document);

        /// <summary>
        /// Less performant if you pass collections, use dedicated overloads instead if you need performance
        /// <para/>
        /// Otherwise automatic redirection will be attempted
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Document"></param>
        public void Update<T>(T Document);
        /// <summary>
        /// Less performant if you pass collections, use dedicated overloads instead if you need performance
        /// <para/>
        /// Otherwise automatic redirection will be attempted
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Document"></param>
        public Task UpdateAsync<T>(T Document);
        public void Update<T>(IEnumerable<T> Document);
        public Task UpdateAsync<T>(IEnumerable<T> Document);
        public void Update<T>(RepeatedField<T> Document);
        public Task UpdateAsync<T>(RepeatedField<T> Document);

        /// <summary>
        /// Less performant if you pass collections, use dedicated overloads instead if you need performance
        /// <para/>
        /// Otherwise automatic redirection will be attempted
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Document"></param>
        public void Upsert<T>(T Document);
        /// <summary>
        /// Less performant if you pass collections, use dedicated overloads instead if you need performance
        /// <para/>
        /// Otherwise automatic redirection will be attempted
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Document"></param>
        public Task UpsertAsync<T>(T Document);
        public void Upsert<T>(IEnumerable<T> Document);
        public Task UpsertAsync<T>(IEnumerable<T> Document);
        public void Upsert<T>(RepeatedField<T> Document);
        public Task UpsertAsync<T>(RepeatedField<T> Document);

        /// <summary>
        /// Less performant if you pass collections, use dedicated overloads instead if you need performance
        /// <para/>
        /// Otherwise automatic redirection will be attempted
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Document"></param>
        public void Delete<T>(T Document);
        /// <summary>
        /// Less performant if you pass collections, use dedicated overloads instead if you need performance
        /// <para/>
        /// Otherwise automatic redirection will be attempted
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Document"></param>
        public Task DeleteAsync<T>(T Document);
        public void Delete<T>(IEnumerable<T> Document);
        public Task DeleteAsync<T>(IEnumerable<T> Document);
        public void Delete<T>(RepeatedField<T> Document);
        public Task DeleteAsync<T>(RepeatedField<T> Document);


        public void SaveChanges();
        public Task SaveChangesAsync();

    }
}

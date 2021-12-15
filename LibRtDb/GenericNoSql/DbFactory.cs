using LibRtDb.DTO;
using LibRtDb.GenericNoSql.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading;

namespace LibRtDb.GenericNoSql
{
    public static class DbFactory
    {

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public static IGenericNoSql Context { get; private set; } = null;

        /// <summary>
        /// Locks thread execution until DB is ready to accept us
        /// </summary>
        public static void EnsureDbStarted()
        {
            try
            {
                log.Debug("EnsureDbStarted Invoked!");

                IGenericNoSql res = null;
                do
                {
                    res = GetContext();
                    if(res == null)
                    {
                        log.Debug("Will wait a bit before attempting next connection!");
                        Thread.Sleep(5000);
                    }
                } 
                while (res == null);


                log.Debug("EnsureDbStarted DONE!");
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
        }

        public static IGenericNoSql GetContext()
        {
            bool isLockAcquired = false;
            try
            {

                if(Context == null)
                {
                    //Acquire GLOBAL lock, across all services
                    //GlobalMutexLocks.MartenDb.WaitOne();
                    isLockAcquired = true;

                    //log.Trace($"Lock Acquired! {nameof(GlobalMutexLocks.MartenDb)}");

                    //var cfg = GetConfigs();
                    var dbDescription = GetConnectionString();

                    log.Debug($"Connection String: {dbDescription.ConnStr}");

                    if(dbDescription.DbType == DbType.MartenPostgres)
                    {
                        Context = new Implementations.MartenDB.MartenDBContextBuilder()
                        .SetConnectionString(dbDescription.ConnStr)
                        .GenerateContext();
                    }
                    else if (dbDescription.DbType == DbType.LiteDB)
                    {
                        Context = new Implementations.LiteDB.LiteDBContextBuilder()
                        .SetConnectionString(dbDescription.ConnStr)
                        .GenerateContext();
                    }
                    else
                    {
                        log.Error("Unhandled DB Type!");
                    }
                    

                    if(isLockAcquired == true)
                    {
                        //Release GLOBAL mutex, so eventually others can continue
                        //GlobalMutexLocks.MartenDb.ReleaseMutex();
                        isLockAcquired = false;

                        //log.Trace($"Lock Released! {nameof(GlobalMutexLocks.MartenDb)}");
                    }

                }
                return Context;

            }
            catch(Exception ex)
            {
                log.Error(ex);
                
                if(isLockAcquired == true)
                {
                    //Anyway ensure to release lock eventually
                    //GlobalMutexLocks.MartenDb.ReleaseMutex();
                    //log.Trace($"Lock Released! {nameof(GlobalMutexLocks.MartenDb)}");
                }
                
                return null;
            }
        }

        private static (int DbType, string ConnStr) GetConnectionString()
        {
            try
            {
                log.Debug("GetConfigs Invoked!");

                string connStr = "host=127.0.0.1;database=postgres;password=pwd;username=postgres;";
                string jsonSettingsFile = "AppSettings.json";

                //this will be the default one as overall more robust
                int dbType = DbType.MartenPostgres;

                if (File.Exists(jsonSettingsFile) == true)
                {

                    log.Debug("JSON Config File Found!");

                    JObject data = JObject.Parse(File.ReadAllText(jsonSettingsFile, Encoding.UTF8));

                    if(data["DbType"] != null)
                    {
                        dbType = data["DbType"].Value<int>();
                    }
                    else
                    {
                        log.Warn($"DbType is not specified! Will use default one: {dbType}");
                    }

                    if(dbType == DbType.MartenPostgres)
                    {
                        connStr = data["ConnectionStrings"]["Postgres"].ToString();
                    }
                    else
                    {
                        connStr = data["ConnectionStrings"]["LiteDb"].ToString();
                    }

                }
                else if (ConfigurationManager.ConnectionStrings["Postgres"] != null)
                {

                    log.Debug("Will try to search in App.config");
                    
                    connStr = ConfigurationManager.ConnectionStrings["Postgres"].ConnectionString;

                }
                else
                {
                    log.Debug($"Will leave to default value: {connStr}");
                }

                return (dbType, connStr);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return (0, null);
            }
            
        }

    }
}

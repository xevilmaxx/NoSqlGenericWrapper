using LibRtDb.DTO.DynamicKeys;
using LibRtDb.DTO.RuntimeConfigs;
using LibRtDb.GenericNoSql;
using LibRtDb.GenericNoSql.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibRtDb.Services
{
    public class DyncamicKeysService
    {

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        #region Read
        public DynamicKey Get(long DeviceId, string KeyName)
        {
            try
            {

                log.Debug($"Get Dynamic Key -> DeviceId: {DeviceId}, KeyName: {KeyName}");

                DynamicKey result = null;

                using (var session = DbFactory.GetContext().QuerySession())
                {

                    result = session.Query<DynamicKey>().Where(
                        x =>
                        x.DeviceId == DeviceId
                        &&
                        x.Key.Equals(KeyName)
                        ).FirstOrDefault();

                }

                log.Debug($"Value: {result?.Value}");

                return result;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        #endregion

        #region Write
        public bool Set(long DeviceId, string KeyName, dynamic KeyValue)
        {
            try
            {

                using (var session = DbFactory.GetContext().LightweightSession())
                {

                    Set(session, DeviceId, KeyName, KeyValue);

                    session.SaveChanges();

                }

                return true;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public bool Set(IGenericNoSqlLightweightSession session, long DeviceId, string KeyName, dynamic KeyValue)
        {
            try
            {

                var result = session.Query<DynamicKey>().Where(x => x.Key.Equals(KeyName)).FirstOrDefault();
                if (result != null)
                {
                    result.Value = KeyValue;
                }
                else
                {
                    result = new DynamicKey()
                    {
                        DeviceId = DeviceId,
                        Key = KeyName,
                        Value = KeyValue,
                        Description = "Generated"
                    };
                }

                session.Upsert(result);

                return true;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public bool Set(List<DynamicKey> keys)
        {
            try
            {

                if (keys == null || keys.Count == 0)
                {
                    log.Debug("No configs passed");
                    return true;
                }

                using (var session = DbFactory.GetContext().LightweightSession())
                {

                    session.Upsert(keys);
                    session.SaveChanges();

                }

                return true;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public bool SetWithoutDuplicates(List<DynamicKey> keys, GetConfigsDTO data)
        {
            try
            {

                if (keys == null || keys.Count == 0)
                {
                    log.Debug("No configs passed");
                    return true;
                }

                using (var session = DbFactory.GetContext().LightweightSession())
                {

                    var allDynKeys = session.Query<DynamicKey>().Where(x => x.DeviceId.Equals(data.DeviceId)).ToList();

                    List<DynamicKey> KeysToAdd = new List<DynamicKey>();
                    foreach (var key in keys)
                    {
                        var isPresent = allDynKeys.Any(x => x.Key.Equals(key.Key));
                        if (isPresent == false)
                        {
                            KeysToAdd.Add(key);
                        }
                    }

                    if (KeysToAdd.Count > 0)
                    {
                        session.Upsert(KeysToAdd);
                        session.SaveChanges();
                    }
                    else
                    {
                        log.Debug("No new dynamic keys to add");
                    }

                }

                return true;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        #endregion

        #region Utils
        #endregion

        #region Delete
        #endregion

    }
}

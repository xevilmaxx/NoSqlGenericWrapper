using LibRtDb.DTO.SerialNumbers;
using LibRtDb.GenericNoSql;
using System;
using System.Collections.Generic;

namespace LibRtDb.Services
{
    public class SerialNumbersService
    {

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        #region Read
        public List<SerialNumber> GetSerialNumber()
        {
            var result = new List<SerialNumber>();
            using (var session = DbFactory.GetContext().QuerySession())
            {
                result = session.Query<SerialNumber>().ToList();
            }
            return result;
        }

        public List<SerialNumber> GetSerialNumber(uint deviceType)
        {
            var result = new List<SerialNumber>();
            using (var session = DbFactory.GetContext().QuerySession())
            {
                result = session.Query<SerialNumber>().Where(x => x.DeviceType == deviceType).ToList();
            }
            return result;
        }
        #endregion

        #region Write
        public void AddSerialNumber(int deviceType, string serialEncypted)
        {
            try
            {

                log.Debug($"AddSerialNumber Invoked! DeviceType: {deviceType}");

                using (var session = DbFactory.GetContext().LightweightSession())
                {
                    var toAdd = new SerialNumber
                    {
                        DeviceType = deviceType,
                        SerNum = serialEncypted,
                        LastChange = DateTime.Now
                    };
                    session.Upsert(toAdd);
                    session.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }

        public void UpsertSerialNumber(List<SerialNumber> data)
        {
            try
            {

                log.Debug($"UpsertSerialNumber Invoked! Records: {data?.Count}");

                using (var session = DbFactory.GetContext().LightweightSession())
                {
                    session.Upsert(data);
                    session.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }


        /// <summary>
        /// Insert list of SerialNumbers without Id header, if you provide Id records on DB will be updated
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateSerialNumber(List<SerialNumber> data)
        {
            try
            {

                using (var session = DbFactory.GetContext().LightweightSession())
                {
                    session.Upsert(data);
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
        #endregion

        #region Utils
        #endregion

        #region Delete
        #endregion

    }
}

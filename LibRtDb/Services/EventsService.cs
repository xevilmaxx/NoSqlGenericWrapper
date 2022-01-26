using Google.Protobuf.Collections;
using LibRtDb.GenericNoSql;
using System;

namespace LibRtDb.Services
{
    public class EventsService
    {

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        #region Read

        //public RepeatedField<DeviceEvent> GetLastEvents(string FromDateStr, int DeviceId)
        //{

        //    try
        //    {

        //        var result = new RepeatedField<DeviceEvent>();

        //        using (var session = DbFactory.GetContext().QuerySession())
        //        {

        //            var FromDate = GenFunc.ReadDate(FromDateStr);

        //            var events = session.Query<DTO.Events.Event>().Where(x =>
        //                x.DeviceId == DeviceId
        //                &&
        //                x.Date.Subtract(FromDate).TotalSeconds > 0
        //            ).ToList();

        //            log.Debug("Found: " + events.Count + " new events");

        //            foreach (var e in events)
        //            {

        //                //just convert
        //                var tmp = new DeviceEvent();
        //                tmp.DeviceId = e.DeviceId.ToString();
        //                tmp.EventDate = GenFunc.WriteDate(e.Date);
        //                tmp.EventType = e.EventType.ToString();

        //                result.Add(tmp);

        //            }

        //        }

        //        return result;

        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(ex, "GetLastEvents -> ");
        //        return null;
        //    }

        //}

        //public RepeatedField<DeviceEvent> GetLastEvents(string FromDateStr)
        //{

        //    try
        //    {

        //        var result = new RepeatedField<DeviceEvent>();

        //        using (var session = DbFactory.GetContext().QuerySession())
        //        {

        //            var FromDate = GenFunc.ReadDate(FromDateStr);

        //            var events = session.Query<DTO.Events.Event>().Where(x =>
        //                x.Date.Subtract(FromDate).TotalSeconds > 0
        //            ).ToList();

        //            log.Debug("Found: " + events.Count + " new events");

        //            foreach (var e in events)
        //            {

        //                //just convert
        //                var tmp = new DeviceEvent();
        //                tmp.DeviceId = e.DeviceId.ToString();
        //                tmp.EventDate = GenFunc.WriteDate(e.Date);
        //                tmp.EventType = e.EventType.ToString();

        //                result.Add(tmp);

        //            }

        //        }

        //        return result;

        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(ex, "GetLastEvents -> ");
        //        return null;
        //    }

        //}
        #endregion

        #region Write
        public bool SaveEvent(long DeviceId, int EventType)
        {
            try
            {

                if (DeviceId < 0 || EventType < 0)
                {
                    log.Debug(string.Format("DeviceId [{0}] or EventType [{1}] is < 0, wont procede further", DeviceId, EventType));
                    return false;
                }

                using (var session = DbFactory.GetContext().LightweightSession())
                {

                    log.Debug($"Registereing event: {EventType} for device: {DeviceId}");

                    var tmpEvent = new DTO.Events.Event
                    {
                        Date = DateTime.Now,
                        DeviceId = DeviceId,
                        EventType = EventType
                    };

                    session.Upsert(tmpEvent);

                    session.SaveChanges();

                }

                return true;

            }
            catch (Exception ex)
            {
                log.Error(ex, "SaveEvent -> ");
                return false;
            }
        }

        //public bool SaveEvent(ClientInfos infos)
        //{
        //    try
        //    {

        //        using (var session = DbFactory.GetContext().LightweightSession())
        //        {

        //            log.Debug($"Registereing event for device: {infos?.DeviceName}");

        //            var tmpEvent = new DTO.Events.Event();
        //            tmpEvent.Date = DateTime.Now;
        //            tmpEvent.DeviceId = infos.DeviceId;
        //            tmpEvent.EventType = -1;

        //            var deviceType = infos.DeviceType;

        //            if (
        //                deviceType == (int)DeviceType.HidRfid
        //                ||
        //                deviceType == (int)DeviceType.NlBarcode
        //                )
        //            {

        //                if (infos.IsActive == true)
        //                {
        //                    tmpEvent.EventType = EventType.ReaderReconnected;
        //                }
        //                else
        //                {
        //                    tmpEvent.EventType = EventType.ReaderCommunicationError;
        //                }

        //            }
        //            /*else if (deviceType == (int)DeviceType.Lcd1602)
        //            {
        //                if (infos.IsActive == true)
        //                {
        //                    tmpEvent.EventType = EventType.ReaderReconnected;
        //                }
        //                else
        //                {
        //                    tmpEvent.EventType = EventType.ReaderCommunicationError;
        //                }
        //            }*/

        //            log.Debug("Event: " + tmpEvent.EventType);

        //            if (tmpEvent.EventType > 0)
        //            {
        //                session.Upsert(tmpEvent);
        //                session.SaveChanges();
        //            }
        //            else
        //            {
        //                log.Debug("Check SaveEvent() in LibRtDb, you probably miss conditions for device: " + infos.DeviceId);
        //            }

        //        }

        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(ex, "SaveEvent -> ");
        //        return false;
        //    }
        //}
        #endregion

        #region Utils
        #endregion

        #region Delete

        public bool DeleteEvents(DateTime DeletePriorTo)
        {
            try
            {

                using (var session = DbFactory.GetContext().LightweightSession())
                {

                    log.Debug($"Will attempt to delete local evants prior to: {DeletePriorTo}");

                    //var removableEvents = session.Query<DTO.Events.Event>().Where(x => x.Date <= DeletePriorTo).ToList();

                    //log.Debug($"Removable Events Count: {removableEvents.Count}");

                    //session.Delete(removableEvents);

                    session.DeleteWhere<DTO.Events.Event>(x => x.Date <= DeletePriorTo);

                    session.SaveChanges();

                }

                return true;

            }
            catch (Exception ex)
            {
                log.Debug(ex, "DeleteEvents -> ");
                return false;
            }
        }

        #endregion

    }
}

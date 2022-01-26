using System;

namespace LibRtDb.DTO.Events
{
    public class Event
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public long DeviceId { get; set; }
        public int EventType { get; set; }
    }
}

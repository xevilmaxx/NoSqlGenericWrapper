using System;

namespace LibRtDb.DTO.SerialNumbers
{
    public class SerialNumber
    {
        public long Id { get; set; }
        public int DeviceType { get; set; }
        public string SerNum { get; set; }
        public DateTime LastChange { get; set; }
        public string Notes { get; set; }
    }
}

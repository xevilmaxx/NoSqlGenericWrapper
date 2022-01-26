using System.Collections.Generic;

namespace LibRtDb.DTO.DeviceConfigs
{
    public class JsonDeviceConfigs
    {
        public long Id { get; set; }
        public int DeviceType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DevConfig> Configs { get; set; }
    }

}

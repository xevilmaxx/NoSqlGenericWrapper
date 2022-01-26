using LibRtDb.DTO.DeviceConfigs;
using LibRtDb.DTO.DynamicKeys;
using System.Collections.Generic;

namespace LibRtDb.DTO.RuntimeConfigs
{
    public class GetConfigsResultDTO
    {
        public List<DevConfig> StaticConfigs { get; set; }
        public List<DynamicKey> DynamicKeys { get; set; }
    }
}

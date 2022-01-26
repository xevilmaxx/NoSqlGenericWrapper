namespace LibRtDb.DTO.RuntimeConfigs
{
    public class GetConfigsDTO
    {
        public long DeviceId { get; set; }
        public int DeviceType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

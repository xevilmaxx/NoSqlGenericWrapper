using LibRtDb.DTO.RuntimeConfigs;
using LibRtDb.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestLibRtDb
{
    public enum EnumTest
    {
        Prop1,
        Prop2
    }

    public static class RunCfgTest
    {
        public static int Param1 { get; set; } = 0;

        [Display(Name = "ParSubstitute")]
        public static EnumTest Para2 { get; set; } = EnumTest.Prop1;
        
        public static EnumTest _para3 = EnumTest.Prop2;
        
        [Display(AutoGenerateField = false)]
        public static int TestHide { get; set; } = -1;

        [Display(GroupName = "DYNAMIC")]
        public static EnumTest _para4 = EnumTest.Prop2;

    }

    public class ConfigTests
    {
        [Test]
        public void CheckConfigsGeneration()
        {

            var cfg = GenericRuntimeConfigs.GetConfigs(typeof(RunCfgTest), 
                new GetConfigsDTO() {
                    DeviceId = 0,
                    DeviceType = 0,
                    Description = "",
                    Name = "" }
                );
            
            Assert.AreEqual(2, cfg.StaticConfigs.Count);
            Assert.AreEqual(1, cfg.DynamicKeys.Count);

        }
    }
}

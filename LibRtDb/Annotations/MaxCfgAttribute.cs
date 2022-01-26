using System;

namespace LibRtDb.Annotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MaxCfgAttribute : Attribute
    {

        /// <summary>
        /// Custom Key Name on DB
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Custom Description on DB
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Will NOT be generated in DB
        /// </summary>
        public bool IsIgnorable { get; set; }

        /// <summary>
        /// Will be stored as Dynamic Key on DB if setted to True
        /// </summary>
        public bool IsDynamicKey { get; set; }

    }
}

namespace LibRtDb.Extensions.Linq.MyEquals
{
    public static class CustomExtensions
    {
        /// <summary>
        /// Overriden by my custom Linq for Marten
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static bool MyEquals(this object src, object dst)
        {
            return true;// src.Equals(dst);
        }

    }
}

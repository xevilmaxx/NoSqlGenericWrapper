using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibRtDb.CustomExceptions
{
    public class CustomNoSqlDbException : Exception
    {

        public CustomNoSqlDbException()
        { }

        public CustomNoSqlDbException(string message)
            : base(message)
        { }

        public CustomNoSqlDbException(string message, Exception innerException)
            : base(message, innerException)
        { }

    }
}

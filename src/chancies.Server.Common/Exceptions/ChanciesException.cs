using System;

namespace chancies.Server.Common.Exceptions
{
    public class ChanciesException
        : Exception
    {
        public ChanciesException()
        {
        }

        public ChanciesException(string message)
            : base(message)
        {
        }

        public ChanciesException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

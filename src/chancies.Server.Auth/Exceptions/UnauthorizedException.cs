using chancies.Server.Common.Exceptions;

namespace chancies.Server.Auth.Exceptions
{
    public class UnauthorizedException
        : ChanciesException
    {
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string message)
            : base(message)
        {
        }
    }
}
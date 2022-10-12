namespace chancies.Server.Common.Exceptions
{
    public class InvalidDataException
        : ChanciesException
    {
        public InvalidDataException(string message)
            : base(message)
        {
        }
    }
}
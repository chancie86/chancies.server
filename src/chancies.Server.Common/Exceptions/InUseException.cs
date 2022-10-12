namespace chancies.Server.Common.Exceptions
{
    public class InUseException
        : ChanciesException
    {
        public InUseException(string message)
            : base(message)
        {}
    }
}

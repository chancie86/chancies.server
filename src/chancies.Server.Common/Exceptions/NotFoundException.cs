namespace chancies.Server.Common.Exceptions
{
    public class NotFoundException
        : System.Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}

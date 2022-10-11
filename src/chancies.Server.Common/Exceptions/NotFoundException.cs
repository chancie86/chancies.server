namespace chancies.Server.Common.Exceptions
{
    public class NotFoundException
        : ChanciesException
    {
        public NotFoundException(string entity, string id)
            : this($"{entity}:{id}")
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}
namespace chancies.Server.Common.Exceptions
{
    public class DuplicateObjectException
        : ChanciesException
    {
        public DuplicateObjectException(string entity, string id)
            : base($"{entity}:{id}")
        {
        }
    }
}
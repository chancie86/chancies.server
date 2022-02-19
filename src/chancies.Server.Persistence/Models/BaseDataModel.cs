namespace chancies.Server.Persistence.Models
{
    public abstract class BaseDataModel<TId>
    {
        public TId Id { get; set; }
        public string Name { get; set; }
        public string Type => GetType().Name;
    }
}

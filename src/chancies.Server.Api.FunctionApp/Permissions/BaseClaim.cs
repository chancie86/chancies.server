namespace chancies.Server.Api.FunctionApp.Permissions
{
    public class BaseClaim
    {
        private readonly string _value;

        protected BaseClaim(string action)
        {
            var entity = GetType().Name.ToLowerInvariant();
            _value = $"{entity}:{action.ToLowerInvariant()}";
        }
        
        public override string ToString()
        {
            return _value;
        }
    }
}

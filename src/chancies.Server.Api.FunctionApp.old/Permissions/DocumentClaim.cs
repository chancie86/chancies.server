namespace chancies.Server.Api.FunctionApp.Permissions
{
    internal class DocumentClaim
        : BaseClaim
    {
        private DocumentClaim(string action)
            : base(action)
        {
        }

        public static DocumentClaim Create { get; } = new(nameof(Create));
        public static DocumentClaim Read { get; } = new(nameof(Read));
        public static DocumentClaim Update { get; } = new(nameof(Update));
        public static DocumentClaim Delete { get; } = new(nameof(Delete));
    }
}

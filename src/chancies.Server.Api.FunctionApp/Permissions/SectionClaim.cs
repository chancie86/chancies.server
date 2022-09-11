namespace chancies.Server.Api.FunctionApp.Permissions
{
    internal class SectionClaim
        : BaseClaim
    {
        private SectionClaim(string action)
            : base(action)
        {
        }

        public static SectionClaim Create { get; } = new(nameof(Create));
        public static SectionClaim Read { get; } = new(nameof(Read));
        public static SectionClaim Update { get; } = new(nameof(Update));
        public static SectionClaim Delete { get; } = new(nameof(Delete));
    }
}

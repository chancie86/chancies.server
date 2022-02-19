using chancies.Server.Persistence.Models;

namespace chancies.Server.Api.Controllers.Public.Section.Dto.Extensions
{
    public static class SectionExtensions
    {
        public static SectionDto ToDto(this Persistence.Models.Section self)
        {
            return new SectionDto
            {
                Id = self.Id,
                Name = self.Name
            };
        }

        public static SectionDto ToDto(this SectionListItem self)
        {
            return new SectionDto
            {
                Id = self.Id,
                Name = self.Name
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using chancies.Server.Api.Controllers.Public.Section.Dto;
using chancies.Server.Api.Controllers.Public.Section.Dto.Extensions;
using chancies.Server.Blog.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace chancies.Server.Api.FunctionApp.Functions.Public
{
    public class SectionFunctions
    {
        private readonly ISectionService _sectionService;

        public SectionFunctions(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        [Function($"{nameof(Public)}_{nameof(SectionFunctions)}_{nameof(List)}")]
        public async Task<IList<SectionDto>> List([HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = $"{Constants.ApiVersion}/public/section")] HttpRequestData req)
        {
            return (await _sectionService.List()).Select(s => s.ToDto()).ToList();
        }

        [Function($"{nameof(Public)}_{nameof(SectionFunctions)}_{nameof(GetSectionById)}")]
        public async Task<SectionDto> GetSectionById(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = $"{Constants.ApiVersion}/public/section/{{sectionId}}")]            Guid sectionId)
        {
            return (await _sectionService.Get(sectionId)).ToDto();
        }
    }
}

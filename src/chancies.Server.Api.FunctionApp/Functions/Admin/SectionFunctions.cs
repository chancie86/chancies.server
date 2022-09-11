using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using chancies.Server.Api.Controllers.Admin.Section.Dto;
using chancies.Server.Api.Controllers.Public.Section.Dto;
using chancies.Server.Api.Controllers.Public.Section.Dto.Extensions;
using chancies.Server.Blog.Interfaces;
using chancies.Server.Persistence.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace chancies.Server.Api.FunctionApp.Functions.Admin
{
    public class SectionFunctions
    {
        private readonly ISectionService _sectionService;

        public SectionFunctions(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        [Authorize(Api.Permissions.Section.Create)]
        [Function($"{nameof(Admin)}{nameof(SectionFunctions)}{nameof(Create)}")]
        public async Task<SectionId> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Post),Route = $"{Constants.ApiVersion}/admin/section")] HttpRequestData req,
            CreateSectionDto dto)
        {
            return await _sectionService.Create(new Section
            {
                Name = dto.Name
            });
        }
    }
}

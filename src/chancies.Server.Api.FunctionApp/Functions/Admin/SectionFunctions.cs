using System;
using System.Net.Http;
using System.Threading.Tasks;
using chancies.Server.Api.Controllers.Admin.Section.Dto;
using chancies.Server.Api.FunctionApp.Extensions;
using chancies.Server.Api.FunctionApp.Functions.Dtos;
using chancies.Server.Blog.Interfaces;
using chancies.Server.Persistence.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [Function($"{nameof(Admin)}_{nameof(SectionFunctions)}_{nameof(Create)}")]
        public async Task<IdDto> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Post),Route = $"{Constants.ApiVersion}/admin/section")] HttpRequestData req)
        {
            var dto = req.ReadBody<CreateSectionDto>();
            var sectionId = await _sectionService.Create(new Section
            {
                Name = dto.Name
            });

            return new IdDto(sectionId);
        }

        [Authorize(Api.Permissions.Section.Update)]
        [Function($"{nameof(Admin)}_{nameof(SectionFunctions)}_{nameof(Update)}")]
        public async Task Update(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Put), Route = $"{Constants.ApiVersion}/admin/section/{{sectionId}}")] HttpRequestData req,
            Guid sectionId)
        {
            var dto = req.ReadBody<CreateSectionDto>();
            await _sectionService.Update(new Section
            {
                Id = sectionId,
                Name = dto.Name
            });
        }

        [Authorize(Api.Permissions.Section.Delete)]
        [Function($"{nameof(Admin)}_{nameof(SectionFunctions)}_{nameof(Delete)}")]
        public async Task Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Delete), Route = $"{Constants.ApiVersion}/admin/section/{{sectionId}}")] HttpRequestData req,
            Guid sectionId)
        {
            await _sectionService.Delete(sectionId);
        }
    }
}

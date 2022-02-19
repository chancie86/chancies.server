using System;
using System.Threading.Tasks;
using chancies.Server.Api.Controllers.Admin.Section.Dto;
using chancies.Server.Blog.Interfaces;
using chancies.Server.Persistence.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace chancies.Server.Api.Controllers.Admin.Section
{
    [Authorize]
    [ApiController]
    [Route("admin/section")]
    public class SectionController
        : ControllerBase
    {
        private readonly ISectionService _sectionService;

        public SectionController(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        [Authorize(Permissions.Section.Create)]
        [HttpPost]
        public async Task<ActionResult<SectionId>> Create(CreateSectionDto dto)
        {
            return await _sectionService.Create(new Persistence.Models.Section
            {
                Name = dto.Name
            });
        }

        [Authorize(Permissions.Section.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _sectionService.Delete(id);
            return base.NoContent();
        }

        [Authorize(Permissions.Section.Update)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateSectionDto payload)
        {
            await _sectionService.Update(new Persistence.Models.Section
            {
                Id = id,
                Name = payload.Name
            });
            return base.NoContent();
        }
    }
}

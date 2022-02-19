using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chancies.Server.Api.Controllers.Public.Section.Dto;
using chancies.Server.Api.Controllers.Public.Section.Dto.Extensions;
using chancies.Server.Blog.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace chancies.Server.Api.Controllers.Public.Section
{
    [ApiController]
    [Route("public/section")]
    public class SectionController
        : ControllerBase
    {
        private readonly ISectionService _sectionService;

        public SectionController(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<SectionDto>>> List()
        {
            return (await _sectionService.List()).Select(s => s.ToDto()).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SectionDto>> GetById(Guid id)
        {
            return (await _sectionService.Get(id)).ToDto();
        }
    }
}

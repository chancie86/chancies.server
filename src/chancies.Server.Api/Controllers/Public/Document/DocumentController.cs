using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chancies.Server.Api.Controllers.Public.Document.Dto;
using chancies.Server.Api.Controllers.Public.Document.Dto.Extensions;
using chancies.Server.Blog.Interfaces;
using chancies.Server.Persistence.Models;
using Microsoft.AspNetCore.Mvc;

namespace chancies.Server.Api.Controllers.Public.Document
{
    [ApiController]
    [Route("public/document")]
    public class DocumentController
        : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IImageService _imageService;

        public DocumentController(IDocumentService documentService, IImageService imageService)
        {
            _documentService = documentService;
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<DocumentListItemDto>>> List()
        {
            return (await _documentService.List()).Select(s => s.ToDocumentListItemDto()).ToArray();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentDto>> GetById(Guid id)
        {
            var document = await _documentService.Get(id);

            if (!document.Published)
            {
                return Forbid();
            }

            return document.ToDocumentDto();
        }

        [HttpGet("{id}/images")]
        public async Task<IList<ImageReference>> ListImages(Guid id)
        {
            var result = await _imageService.List(id);
            return result;
        }
    }
}

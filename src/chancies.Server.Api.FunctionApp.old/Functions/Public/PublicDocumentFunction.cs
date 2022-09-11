using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using chancies.Server.Api.Controllers.Public.Document.Dto;
using chancies.Server.Api.Controllers.Public.Document.Dto.Extensions;
using chancies.Server.Blog.Interfaces;
using chancies.Server.Persistence.Models;

namespace chancies.Server.Api.FunctionApp.Functions.Public
{
    public class PublicDocumentFunction
    {
        private readonly IDocumentService _documentService;
        private readonly IImageService _imageService;

        public PublicDocumentFunction(
            IDocumentService documentService,
            IImageService imageService)
        {
            _documentService = documentService;
            _imageService = imageService;
        }
        
        [FunctionName($"{nameof(PublicDocumentFunction)}{nameof(GetDocuments)}")]
        public async Task<ActionResult<IList<DocumentListItemDto>>> GetDocuments(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = "public/document")]
            HttpRequest req)
        {
            return (await _documentService.List()).Select(s => s.ToDocumentListItemDto()).ToList();
        }

        [FunctionName($"{nameof(PublicDocumentFunction)}{nameof(GetDocumentById)}")]
        public async Task<ActionResult<DocumentDto>> GetDocumentById(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = "public/document/{documentId}")]
            HttpRequest req,
            Guid documentId)
        {
            var document = await _documentService.Get(documentId);

            if (!document.Published)
            {
                return new ForbidResult();
            }

            return document.ToDocumentDto();
        }

        [FunctionName($"{nameof(PublicDocumentFunction)}{nameof(GetDocumentImages)}")]
        public async Task<ActionResult<IList<ImageReference>>> GetDocumentImages(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = "public/document/{documentId}/images")]
            HttpRequest req,
            Guid documentId)
        {
            return (await _imageService.List(documentId)).ToList();
        }
    }
}

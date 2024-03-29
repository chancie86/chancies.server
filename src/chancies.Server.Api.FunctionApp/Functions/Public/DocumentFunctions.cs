using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using chancies.Server.Api.Controllers.Public.Document.Dto;
using chancies.Server.Api.Controllers.Public.Document.Dto.Extensions;
using chancies.Server.Blog.Interfaces;
using chancies.Server.Persistence.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace chancies.Server.Api.FunctionApp.Functions.Public
{
    public class DocumentFunctions
    {
        private readonly IDocumentService _documentService;
        private readonly IImageService _imageService;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public DocumentFunctions(
            IDocumentService documentService,
            IImageService imageService,
            JsonSerializerOptions jsonSerializerOptions)
        {
            _documentService = documentService;
            _imageService = imageService;
            _jsonSerializerOptions = jsonSerializerOptions;
        }
        
        [Function($"{nameof(Public)}_{nameof(DocumentFunctions)}_{nameof(List)}")]
        public async Task<IList<DocumentListItemDto>> List(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = $"{Constants.ApiVersion}/public/document")]
            HttpRequestData req)
        {
            return (await _documentService.List()).Select(s => s.ToDocumentListItemDto()).ToList();
        }

        [Function($"{nameof(Public)}_{nameof(DocumentFunctions)}_{nameof(GetDocumentById)}")]
        public async Task<HttpResponseData> GetDocumentById(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = $"{Constants.ApiVersion}/public/document/{{documentId}}")]
            HttpRequestData req,
            Guid documentId)
        {
            var document = await _documentService.Get(documentId);

            if (!document.Published)
            {
                return req.CreateResponse(HttpStatusCode.Forbidden);
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            var dto = document.ToDocumentDto();
            var json = JsonSerializer.Serialize(dto, _jsonSerializerOptions);
            await response.WriteStringAsync(json);

            return response;
        }

        [Function($"{nameof(Public)}_{nameof(DocumentFunctions)}_{nameof(GetDocumentImages)}")]
        public async Task<IList<ImageReference>> GetDocumentImages(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = $"{Constants.ApiVersion}/public/document/{{documentId}}/images")]
            HttpRequestData req,
            Guid documentId)
        {
            return (await _imageService.List(documentId)).ToList();
        }
    }
}

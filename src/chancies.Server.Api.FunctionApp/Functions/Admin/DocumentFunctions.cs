using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using chancies.Server.Api.Controllers.Admin.Document.Dto;
using chancies.Server.Api.Controllers.Public.Document.Dto.Extensions;
using chancies.Server.Api.FunctionApp.Extensions;
using chancies.Server.Api.FunctionApp.Functions.Dtos;
using chancies.Server.Blog.Interfaces;
using chancies.Server.Common.Extensions;
using chancies.Server.Persistence.Models;
using HttpMultipartParser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace chancies.Server.Api.FunctionApp.Functions.Admin
{
    public class DocumentFunctions
    {
        private readonly IDocumentService _documentService;
        private readonly IImageService _imageService;
        private readonly ILogger<DocumentFunctions> _logger;

        public DocumentFunctions(
            IDocumentService documentService,
            IImageService imageService,
            ILogger<DocumentFunctions> logger)
        {
            _documentService = documentService;
            _imageService = imageService;
            _logger = logger;
        }

        [Authorize(Api.Permissions.Document.Read)]
        [Function($"{nameof(Admin)}_{nameof(DocumentFunctions)}_{nameof(GetDocumentById)}")]
        public async Task<HttpResponseData> GetDocumentById(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = $"{Constants.ApiVersion}/admin/document/{{documentId}}")]
            HttpRequestData req,
            Guid documentId)
        {
            var document = await _documentService.Get(documentId);
            
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            var dto = document.ToDocumentDto();
            var json = JsonSerializer.Serialize(dto);
            await response.WriteStringAsync(json);

            return response;
        }

        [Authorize(Api.Permissions.Document.Create)]
        [Function($"{nameof(Admin)}_{nameof(DocumentFunctions)}_{nameof(Create)}")]
        public async Task<IdDto> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Post),Route = $"{Constants.ApiVersion}/admin/document")] HttpRequestData req)
        {
            var dto = req.ReadBody<CreateDocumentDto>();
            var documentId = await _documentService.Create(dto.ToModel());
            return new IdDto(documentId);
        }

        [Authorize(Api.Permissions.Document.Update)]
        [Function($"{nameof(Admin)}_{nameof(DocumentFunctions)}_{nameof(Update)}")]
        public async Task<HttpResponseData> Update(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Put), Route = $"{Constants.ApiVersion}/admin/document/{{documentId}}")] HttpRequestData req,
            Guid documentId)
        {
            var payload = req.ReadBody<CreateDocumentDto>();
            await _documentService.Update(new Document
            {
                Id = documentId,
                Name = payload.Name,
                Elements = payload.Elements,
                SectionId = payload.SectionId
            });

            return req.CreateResponse(HttpStatusCode.OK);
        }

        [Authorize(Api.Permissions.Document.Delete)]
        [Function($"{nameof(Admin)}_{nameof(DocumentFunctions)}_{nameof(Delete)}")]
        public async Task<HttpResponseData> Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Delete), Route = $"{Constants.ApiVersion}/admin/document/{{documentId}}")] HttpRequestData req,
            Guid documentId)
        {
            await _documentService.Delete(documentId);
            return req.CreateResponse(HttpStatusCode.OK);
        }

        [Authorize(Api.Permissions.Document.Update)]
        [Function($"{nameof(Admin)}_{nameof(DocumentFunctions)}_{nameof(UploadImage)}")]
        public async Task<HttpResponseData> UploadImage(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Post), Route = $"{Constants.ApiVersion}/admin/document/{{documentId}}/images")] HttpRequestData req,
            Guid documentId)
        {
            var parsedFormBody = await MultipartFormDataParser.ParseAsync(req.Body);

            if (parsedFormBody.Files.Count == 0)
            {
                throw new InvalidOperationException("No files received from the upload");
            }

            parsedFormBody.Files.ForEach(ValidateImageFile);

            foreach (var formFile in parsedFormBody.Files)
            {
                await using var stream = formFile.Data;
                await _imageService.Upload(documentId, stream, formFile.FileName);
            }
            
            return req.CreateResponse(HttpStatusCode.OK);
        }

        [Authorize(Api.Permissions.Document.Delete)]
        [Function($"{nameof(Admin)}_{nameof(DocumentFunctions)}_{nameof(DeleteImage)}")]
        public async Task<HttpResponseData> DeleteImage(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Delete), Route = $"{Constants.ApiVersion}/admin/document/{{documentId}}/images")] HttpRequestData req,
            Guid documentId,
            string filePath)
        {
            await _imageService.Delete(documentId, filePath);
            return req.CreateResponse(HttpStatusCode.OK);
        }

        [Authorize(Api.Permissions.Document.Update)]
        [Function($"{nameof(Admin)}_{nameof(DocumentFunctions)}_{nameof(Publish)}")]
        public async Task<HttpResponseData> Publish(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Put), Route = $"{Constants.ApiVersion}/admin/document/{{documentId}}/publish")] HttpRequestData req,
            Guid documentId,
            bool publish)
        {
            await _documentService.Publish(documentId, publish);
            return req.CreateResponse(HttpStatusCode.OK);
        }

        private static void ValidateImageFile(FilePart formFile)
        {
            if (formFile.Data.Length == 0)
            {
                throw new Common.Exceptions.InvalidDataException($"Empty file. Name: {formFile.Name}, FileName: {formFile.FileName}");
            }

            if (!formFile.ContentType.StartsWith("image/"))
            {
                throw new Common.Exceptions.InvalidDataException($"Unsupported content type: {formFile.ContentType}");
            }
        }
    }
}

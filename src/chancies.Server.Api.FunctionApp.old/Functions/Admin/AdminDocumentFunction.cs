using System;
using System.Threading.Tasks;
using chancies.Server.Api.Controllers.Admin.Document.Dto;
using chancies.Server.Api.Controllers.Public.Document.Dto;
using chancies.Server.Api.Controllers.Public.Document.Dto.Extensions;
using chancies.Server.Api.FunctionApp.Permissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using chancies.Server.Auth.Interfaces;
using chancies.Server.Blog.Interfaces;
using chancies.Server.Persistence.Models;

namespace chancies.Server.Api.FunctionApp.Functions.Admin
{
    public class AdminDocumentFunction
        : BaseFunction
    {
        private const string BaseRoute = "v2/admin/document";

        private readonly IDocumentService _documentService;

        public AdminDocumentFunction(
            IAuthenticator authenticator,
            IDocumentService documentService)
            : base(authenticator)
        {
            _documentService = documentService;
        }
        
        [FunctionName($"{nameof(AdminDocumentFunction)}{nameof(GetDocumentById)}")]
        public async Task<ActionResult<DocumentDto>> GetDocumentById(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = "v2/admin/document/{documentId}")]
            HttpRequest req,
            Guid documentId)
        {
            return await InvokeAuthenticatedAsync(
                req,
                async () =>
                {
                    var document = await _documentService.Get(documentId);
                    return document.ToDocumentDto();
                },
                DocumentClaim.Create);
        }

        [FunctionName($"{nameof(AdminDocumentFunction)}{nameof(CreateDocument)}")]
        public async Task<ActionResult<DocumentId>> CreateDocument(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Post),
                Route = BaseRoute)]
            HttpRequest req,
            CreateDocumentDto dto)
        {
            return await InvokeAuthenticatedAsync(
                req,
                async () =>
                {
                    return await _documentService.Create(dto.ToModel());
                }
            );
        }

        [FunctionName($"{nameof(AdminDocumentFunction)}{nameof(DeleteDocument)}")]
        public async Task<ActionResult> DeleteDocument(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Post),
                Route = $"{BaseRoute}/{{documentId}}")]
            HttpRequest req,
            Guid documentId)
        {
            return await InvokeAuthenticatedAsync(
                req,
                async () =>
                {
                    await _documentService.Delete(documentId);
                    return new NoContentResult();
                }
            );
        }
    }
}

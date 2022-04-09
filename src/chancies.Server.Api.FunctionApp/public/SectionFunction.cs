using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chancies.Server.Api.Controllers.Public.Section.Dto;
using chancies.Server.Api.Controllers.Public.Section.Dto.Extensions;
using chancies.Server.Blog.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace chancies.Server.Api.FunctionApp.Public
{
    public class SectionFunction
    {
        private readonly ISectionService _sectionService;

        public SectionFunction(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        [FunctionName(nameof(GetSections))]
        public async Task<ActionResult<IList<SectionDto>>> GetSections(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = "public/section")]
            HttpRequest req)
        {
            return (await _sectionService.List()).Select(s => s.ToDto()).ToList();

            //log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            //string responseMessage = string.IsNullOrEmpty(name)
            //    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //    : $"Hello, {name}. This HTTP triggered function executed successfully.";

            //return new OkObjectResult(responseMessage);
        }

        [FunctionName(nameof(GetSectionById))]
        public async Task<ActionResult<SectionDto>> GetSectionById(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = "public/section/{sectionId}")]
            HttpRequest req,
            Guid sectionId)
        {
            return (await _sectionService.Get(sectionId)).ToDto();
        }
    }
}

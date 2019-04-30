using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using GitLab.Api.Extender.Models;
using GitLab.Api.Extender.Infrastructure;
using GitLab.Api.Extender.Services;
using GitLab.Api.Extender.Helpers;

namespace GitLab.Api.Extender.Controllers
{
    [Produces("application/json")]
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : Controller
    {
        private readonly IGitLabService _gitLabService;
        private readonly IMimeMappingService _mimeMappingService;

        public ProjectsController(IGitLabService gitLabService,
            IMimeMappingService mimeMappingService)
        {
            _gitLabService = gitLabService;
            _mimeMappingService = mimeMappingService;
        }

        /// <summary>
        /// Get project id
        /// </summary>
        /// <returns>Project id on success, otherwise 500 error</returns>
        [AccessKey]
        [HttpGet("id")]
        public async Task<GetProjectIdResponse> GetProjectId([FromQuery] GetProjectIdRequest request)
        {
            var id = await _gitLabService.GetProjectId(request.HttpUrlToRepo);
            if (!id.HasValue)
            {
                throw new Exception($"Project id was not found for {request.HttpUrlToRepo}");
            }

            return new GetProjectIdResponse { Id = id.Value };
        }

        /// <summary>
        /// Adds tag to repo
        /// </summary>
        /// <returns>200 http code on success, otherwise 500 error</returns>
        [AccessKey]
        [HttpPost("repository/tags")]
        public async Task Add([FromBody] AddTagRequest request)
        {
            await _gitLabService.AddTag(request.HttpUrlToRepo, request.Ref, request.TagName, request.Message, request.ReleaseDescription);
        }

        /// <summary>
        /// Get raw file from repo
        /// </summary>
        /// <returns>200 http code on success, otherwise 500 error</returns>
        [AccessKey]
        [HttpGet("repository/files/raw")]
        public async Task<FileStreamResult> GetFile([FromQuery] GetFileRequest request)
        {
            var stream = await _gitLabService.GetFileStream(request.HttpUrlToRepo, request.Ref, request.Path);
            var fileName = UriHelper.GetFileNameFromUrl(request.Path);
            var contentType = _mimeMappingService.Map(fileName);

            return new FileStreamResult(stream, contentType);
        }
    }
}
using GitLab.Api.Extender.Helpers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GitLab.Api.Extender.Services
{
    public class GitLabService : IGitLabService
    {
        private readonly GitLabSettings _settings;
        private readonly IMemoryCache _memoryCache;

        public GitLabService(GitLabSettings settings, IMemoryCache memoryCache)
        {
            _settings = settings ?? throw new ArgumentException($"App__GitLab__Url and App__GitLab__Token must be set");
            if (string.IsNullOrEmpty(_settings.Url))
            {
                throw new ArgumentException("App__GitLab__Url can not bu null or empty");
            }
            if (string.IsNullOrEmpty(_settings.Token))
            {
                throw new ArgumentException($"App__GitLab__Token can not bu null or empty");
            }

            _settings.Url = _settings.Url.TrimEnd('/');
            _memoryCache = memoryCache;
        }

        public async Task<int?> GetProjectId(string httpUrlToRepo)
        {
            if (!_memoryCache.TryGetValue(httpUrlToRepo, out int? projectId))
            {
                projectId = await GetProjectIdInternal(httpUrlToRepo);
                if (projectId != null)
                {
                    _memoryCache.Set(httpUrlToRepo, projectId, TimeSpan.FromDays(1));
                }
            }

            return projectId;
        }

        private async Task<int?> GetProjectIdInternal(string httpUrlToRepo)
        {
            var page = 1;
            var url = $"{_settings.Url}/v4/projects";
            var headers = new { PRIVATE_TOKEN = _settings.Token };

            while (true)
            {
                var data = new { page, per_page = 100, simple = true };
                var projects = await FlurlHelper.GetJsonAsync<IEnumerable<Project>>(url, data, headers);
                if (!projects.Any())
                {
                    return null;
                }

                var project = projects.FirstOrDefault(f => f.http_url_to_repo.StartsWith(httpUrlToRepo, StringComparison.CurrentCultureIgnoreCase));
                if (project != null)
                {
                    return project.id;
                }

                page++;
            }
        }

        public async Task AddTag(string httpUrlToRepo, string refName, string tagName, string message, string releaseDescription)
        {
            var projectId = await GetProjectId(httpUrlToRepo);
            if (!projectId.HasValue)
            {
                throw new Exception($"Project id was not found for {httpUrlToRepo}");
            }

            var url = $"{_settings.Url}/v4/projects/{projectId}/repository/tags";
            var headers = new { PRIVATE_TOKEN = _settings.Token };
            var data = new Dictionary<string, object>
            {
                { "ref", refName },
                { "tag_name", tagName },
                { "message", message },
                { "release_description", releaseDescription }
            };

            await FlurlHelper.PostJsonAsync(url, data, headers);
        }

        public async Task<Stream> GetFileStream(string httpUrlToRepo, string refName, string path)
        {
            var projectId = await GetProjectId(httpUrlToRepo);
            if (!projectId.HasValue)
            {
                throw new Exception($"Project id was not found for {httpUrlToRepo}");
            }

            var url = $"{_settings.Url}/v4/projects/{projectId}/repository/files/{HttpUtility.UrlEncode(path)}/raw?ref={refName}";
            var headers = new { PRIVATE_TOKEN = _settings.Token };

            return await FlurlHelper.GetFileStream(url, headers);
        }

        private class Project
        {
            public int id { get; set; }
            public string http_url_to_repo { get; set; }
        }
    }
}
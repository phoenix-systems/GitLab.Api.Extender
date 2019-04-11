using System.Threading.Tasks;

namespace GitLab.Api.Extender.Services
{
    public interface IGitLabService
    {
        Task<int?> GetProjectId(string httpUrlToRepo);
        Task AddTag(string httpUrlToRepo, string refName, string tagName, string message, string releaseDescription);
    }
}
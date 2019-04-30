using Microsoft.AspNetCore.StaticFiles;

namespace GitLab.Api.Extender.Services
{
    public class MimeMappingService : IMimeMappingService
    {
        private readonly FileExtensionContentTypeProvider _contentTypeProvider;

        public MimeMappingService()
        {
            _contentTypeProvider = new FileExtensionContentTypeProvider();
        }

        public string Map(string fileName)
        {
            if (!_contentTypeProvider.TryGetContentType(fileName, out string contentType))
            {
                contentType = "text/plain";
            }

            return contentType;
        }
    }
}
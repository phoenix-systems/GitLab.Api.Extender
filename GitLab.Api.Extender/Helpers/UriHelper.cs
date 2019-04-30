using System;
using System.IO;

namespace GitLab.Api.Extender.Helpers
{
    public class UriHelper
    {
        readonly static Uri SomeBaseUri = new Uri("http://canbeanything");

        public static string GetFileNameFromUrl(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                uri = new Uri(SomeBaseUri, url);
            }

            return Path.GetFileName(uri.LocalPath);
        }
    }
}
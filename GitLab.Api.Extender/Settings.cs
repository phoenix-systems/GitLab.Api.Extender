namespace GitLab.Api.Extender
{
    public class Settings
    {
        public AppSettings App { get; set; }
    }

    public class AppSettings
    {
        public AccessKeySettings Access { get; set; }
        public GitLabSettings Gitlab { get; set; }
    }

    public class AccessKeySettings
    {
        /// <summary>
        /// Key to secure api endpoints
        /// </summary>
        public string Key { get; set; }
    }

    public class GitLabSettings
    {
        /// <summary>
        /// Url to gitlab api, like: https://example.com/api
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Token that have access to gitlab projects
        /// </summary>
        public string Token { get; set; }
    }
}
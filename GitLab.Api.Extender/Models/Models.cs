﻿using FluentValidation;

namespace GitLab.Api.Extender.Models
{
    public class GetProjectIdRequest
    {
        /// <summary>
        /// The url to gitlab repo. Must start with http:// or https://. Like: http://example.com/app/app-client.git
        /// </summary>
        public string HttpUrlToRepo { get; set; }
    }

    public class GetProjectIdRequestValidator : AbstractValidator<GetProjectIdRequest>
    {
        public GetProjectIdRequestValidator()
        {
            RuleFor(p => p.HttpUrlToRepo)
                .NotEmpty()
                .Must(f => f.ValidateUrl()).WithMessage("Must start with http:// or https://");
        }
    }

    public class AddTagRequest
    {
        /// <summary>
        /// The url to gitlab repo. Must start with http:// or https://. Like: http://example.com/app/app-client.git
        /// </summary>
        public string HttpUrlToRepo { get; set; }

        /// <summary>
        /// Branch name, another tag name or commit SHA. Like: "master", "refs/heads/master", ...
        /// </summary>
        public string Ref { get; set; }

        /// <summary>
        /// The name of a tag
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Annotatation for tag
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Release notes to the git tag
        /// </summary>
        public string ReleaseDescription { get; set; }
    }

    public class AddTagRequestValidator : AbstractValidator<AddTagRequest>
    {
        public AddTagRequestValidator()
        {
            RuleFor(p => p.HttpUrlToRepo)
                .NotEmpty()
                .Must(f => f.ValidateUrl()).WithMessage("Must start with http:// or https://");

            RuleFor(p => p.Ref).NotEmpty();
            RuleFor(p => p.TagName).NotEmpty();
        }
    }

    public class GetProjectIdResponse
    {
        public int Id { get; set; }
    }

    public class GetFileRequest
    {
        /// <summary>
        /// The url to gitlab repo. Must start with http:// or https://. Like: http://example.com/app/app-client.git
        /// </summary>
        public string HttpUrlToRepo { get; set; }

        /// <summary>
        /// Branch name, another tag name or commit SHA. Like: "master", "refs/heads/master", ...
        /// </summary>
        public string Ref { get; set; }

        /// <summary>
        /// The path to file. Like: app/docker-compose.yaml
        /// </summary>
        public string Path { get; set; }
    }

    public class GetFileRequestValidator : AbstractValidator<GetFileRequest>
    {
        public GetFileRequestValidator()
        {
            RuleFor(p => p.HttpUrlToRepo)
                .NotEmpty()
                .Must(f => f.ValidateUrl()).WithMessage("Must start with http:// or https://");

            RuleFor(p => p.Ref).NotEmpty();
            RuleFor(p => p.Path).NotEmpty();
        }
    }

    static class ValidationsExtensions
    {
        public static bool ValidateUrl(this string value)
        {
            if (value != null)
            {
                return value.ToLower().StartsWith("http://") || value.ToLower().StartsWith("https://");
            }

            return true;
        }
    }
}
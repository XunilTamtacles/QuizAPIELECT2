using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace QuizAPIELECT2.Utils
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private const string HEADER_NAME = "X-API-KEY";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var services = context.HttpContext.RequestServices;

            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<ApiKeyAuthorizeAttribute>>();

            
            if (!context.HttpContext.Request.Headers.TryGetValue(HEADER_NAME, out var extractedApiKey))
            {
                logger.LogWarning("No API key provided. Path: {Path}",
                    context.HttpContext.Request.Path);

                context.Result = new UnauthorizedObjectResult("API Key was not provided.");
                return;
            }

           
            var apiKeys = configuration
                .GetSection("Security:ApiKeys")
                .Get<List<ApiKeySetting>>();

            if (apiKeys == null || !apiKeys.Any())
            {
                logger.LogError("API Keys not configured .");

                context.Result = new ObjectResult("Server configuration error.")
                {
                    StatusCode = 500
                };
                return;
            }

           
            var matchedApiKey = apiKeys.FirstOrDefault(apiKey =>
                string.Equals(apiKey.Value, extractedApiKey.ToString(), StringComparison.OrdinalIgnoreCase));

            if (matchedApiKey == null)
            {
                logger.LogWarning("Invalid API key used. Path: {Path}",
                    context.HttpContext.Request.Path);

                context.Result = new UnauthorizedObjectResult("Invalid API Key.");
                return;
            }

            
            var expiresAt = matchedApiKey.CreatedAt.ToUniversalTime()
                            .AddDays(matchedApiKey.ValidForDays);

            if (expiresAt <= DateTime.UtcNow)
            {
                logger.LogWarning("Expired API key used: {KeyName}, Path: {Path}",
                    matchedApiKey.Name,
                    context.HttpContext.Request.Path);

                context.Result = new UnauthorizedObjectResult("API Key is expired.");
                return;
            }

           
        }
    }

    public class ApiKeySetting
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int ValidForDays { get; set; }
    }
}
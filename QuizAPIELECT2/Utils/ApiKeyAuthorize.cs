using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QuizAPIELECT2.Utils
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private const string HEADER_NAME = "X-API-KEY";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

          
            if (!context.HttpContext.Request.Headers.TryGetValue(HEADER_NAME, out var apiKey))
            {
                context.Result = new UnauthorizedObjectResult("API Key is required.");
                return;
            }

            var validKey = config["Security:ApiKey"];

            if (string.IsNullOrEmpty(validKey))
            {
                context.Result = new ObjectResult("Server misconfiguration.")
                {
                    StatusCode = 500
                };
                return;
            }

            
            if (apiKey.ToString() != validKey)
            {
                context.Result = new UnauthorizedObjectResult("Invalid API Key.");
                return;
            }

           
            var expiryDays = int.TryParse(config["Security:ApiKeyExpiryDays"], out var days)
                ? days
                : 30;

            var createdAt = DateTime.Parse(config["Security:ApiKeyCreatedAt"] ?? DateTime.UtcNow.ToString());

            if (DateTime.UtcNow > createdAt.AddDays(expiryDays))
            {
                context.Result = new UnauthorizedObjectResult("API Key expired.");
                return;
            }
        }
    }
}
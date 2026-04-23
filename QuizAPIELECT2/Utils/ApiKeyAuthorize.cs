using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QuizAPIELECT2.Utils
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthorize : Attribute, IAuthorizationFilter
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

            
            var validKeys = config.GetSection("Security:ApiKeys")
                                  .Get<List<ApiKeySetting>>();

            if (validKeys == null || validKeys.Count == 0)
            {
                context.Result = new ObjectResult("Server configuration error.")
                {
                    StatusCode = 500
                };
                return;
            }

            var matchedKey = validKeys.FirstOrDefault(k =>
                k.Value == apiKey.ToString());

            if (matchedKey == null)
            {
                context.Result = new UnauthorizedObjectResult("Invalid API Key.");
                return;
            }

            
            var expiryDate = matchedKey.CreatedAt.AddDays(matchedKey.ValidForDays);

            if (DateTime.UtcNow > expiryDate)
            {
                context.Result = new UnauthorizedObjectResult("API Key expired.");
                return;
            }
        }
    }

  
    public class ApiKeySetting
    {
        public string Name { get; set; } = "";
        public string Value { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public int ValidForDays { get; set; }
    }
}
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;
using QuizAPIELECT2.Utils;
using System.Security.Claims;

namespace QuizAPIELECT2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    RoleClaimType = ClaimTypes.Role
                };
            });

            builder.Services.AddAuthorization();

            
            builder.Services.AddRateLimiter(options =>
            {
                
                options.AddPolicy("LoginPolicy", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        "login",
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 3,
                            Window = TimeSpan.FromMinutes(1)
                        }));

                
                options.AddPolicy("TaskPolicy", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        "tasks",
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10,
                            Window = TimeSpan.FromMinutes(1)
                        }));
            });

            
            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            
            app.UseRateLimiter();     
            app.UseAuthentication();  
            app.UseAuthorization();   

            app.MapControllers();

            app.Run();
        }
    }
}
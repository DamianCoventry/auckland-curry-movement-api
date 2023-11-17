using auckland_curry_movement_api.DatabaseContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

namespace auckland_curry_movement_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Logging.ClearProviders();
                builder.Logging
                    .SetMinimumLevel(LogLevel.Trace)
                    .AddApplicationInsights(tc => { tc.ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]; }, lo => { })
                    .AddAzureWebAppDiagnostics()
                    .AddDebug()
                    .AddConsole();

                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                string connectionString = builder.Configuration.GetConnectionString("SQLAZURECONNSTR_AZURE_SQL_CONNECTIONSTRING");
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_AZURE_SQL_CONNECTIONSTRING") ?? string.Empty;
                    if (string.IsNullOrEmpty(connectionString))
                        throw new Exception("Unable to locate valid database connection string");
                }
                builder.Services.AddDbContext<AcmDatabaseContext>(options => options.EnableDetailedErrors().UseSqlServer(connectionString));

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Caught an exception, the process will end.\n" + ex.ToString());
            }
        }
    }
}
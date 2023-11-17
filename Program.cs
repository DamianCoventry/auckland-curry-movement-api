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
                    .SetMinimumLevel(LogLevel.Debug)
                    .AddApplicationInsights(tc => { tc.ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]; }, lo => { })
                    .AddDebug().AddConsole();

                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddDbContext<AcmDatabaseContext>(
                    options => options.EnableDetailedErrors().UseSqlServer(
                        builder.Configuration.GetConnectionString("SQLAZURECONNSTR_AZURE_SQL_CONNECTIONSTRING")));

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
                System.Diagnostics.Trace.WriteLine("Caught an exception, the process will end.\n" + ex.ToString());
            }
        }
    }
}
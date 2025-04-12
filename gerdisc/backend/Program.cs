using saga.Infrastructure.Providers;
using saga.Infrastructure.Providers.Interfaces;
using saga.Infrastructure.Repositories;
using saga.Properties;
using saga.Services;
using saga.Services.Interfaces;
using saga.Settings;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.Jobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using backend.Infrastructure.Validations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Saga", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.OperationFilter<SecurityRequirementsOperationFilter>();
    c.DocumentFilter<BasePathDocumentFilter>();
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var settings = new AppSettings();
var connectionString = $"Host={settings.PostgresServer};Username={settings.PostgresUser};Password={settings.PostgresPassword};Database={settings.PostgresDb}";

var signingConfig = new SigningConfiguration(settings.SinginKey);

builder.Services.AddDbContext<ContexRepository>(options =>
{
    options.UseNpgsql(connectionString);
}, ServiceLifetime.Scoped);
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddSingleton<ISigningConfiguration>(signingConfig);
builder.Services.AddSingleton<ISettings>(settings);
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
RegisterValidations(builder.Services);
RegisterServices(builder.Services);
builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingConfig.Key
        };
    });

builder.Services.AddHangfireServer();
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(connectionString));

var app = builder.Build();

app.UsePathBase("/api");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Saga V1");
    c.DefaultModelsExpandDepth(-1);
    c.RoutePrefix = string.Empty;
    c.DocumentTitle = "Saga API Documentation";
    c.EnableDeepLinking();
    c.DisplayRequestDuration();
});

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    PrefixPath = string.Empty,
    Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
});

app.UseMiddleware<UserContextMiddleware>();
app.UseMiddleware<LogRequest>();
RecurringJob.AddOrUpdate<StudentsFinishing>("daily-job", x => x.ExecuteAsync(null), Cron.Daily);

app.MapControllers();

app.Run();

void RegisterValidations(IServiceCollection services)
{
    services.AddScoped<Validations>();
}

void RegisterServices(IServiceCollection services)
{

    services.AddScoped<ICourseService, CourseService>();
    services.AddScoped<IStudentService, StudentService>();
    services.AddScoped<IProjectService, ProjectService>();
    services.AddScoped<IResearchLineService, ResearchLineService>();
    services.AddScoped<IProfessorService, ProfessorService>();
    services.AddScoped<IExternalResearcherService, ExternalResearcherService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IOrientationService, OrientationService>();
    services.AddScoped<IExtensionService, ExtensionService>();
}

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
using saga.Infrastructure.Validations;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

// Swagger
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

// Controllers & JSON (enums as strings)
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var settings = new AppSettings();
var connectionString = $"Host={settings.PostgresServer};Username={settings.PostgresUser};Password={settings.PostgresPassword};Database={settings.PostgresDb}";

var signingConfig = new SigningConfiguration(settings.SinginKey);

// DbContext (+dev diagnostics)
builder.Services.AddDbContext<ContexRepository>(options =>
{
    options.UseNpgsql(connectionString);
#if DEBUG
    options.EnableSensitiveDataLogging();   // log SQL params (DEV only)
    options.EnableDetailedErrors();         // richer EF exceptions (DEV only)
#endif
}, ServiceLifetime.Scoped);

// DI
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddSingleton<ISigningConfiguration>(signingConfig);
builder.Services.AddSingleton<ISettings>(settings);
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
RegisterValidations(builder.Services);
RegisterServices(builder.Services);

builder.Services.AddAuthorization();

// Auth
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

// Hangfire
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(connectionString));
builder.Services.AddHangfireServer();

var app = builder.Build();

app.UsePathBase("/api");

// Dev exception page (still useful in DEV)
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Our middlewares early so they see everything
app.UseMiddleware<ExceptionHandlingMiddleware>(); // returns ProblemDetails + traceId on 500
app.UseMiddleware<LogRequest>();                  // logs request/response with correlation id

// Swagger
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

// Pipeline
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Hangfire dashboard
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    PrefixPath = string.Empty,
    Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
});

// Endpoints
app.MapControllers();

// Auto-migrate on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ContexRepository>();
    db.Database.Migrate();
}

// Recurring jobs
RecurringJob.AddOrUpdate<StudentsFinishing>("daily-job", x => x.ExecuteAsync(null), Cron.Daily);

app.Run();

void RegisterValidations(IServiceCollection services)
{
    services.AddScoped<UserValidator>();
    services.AddScoped<OrientationValidator>();
    services.AddScoped<StudentValidator>();
    services.AddScoped<ProfessorValidator>();
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

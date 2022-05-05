using Albelli.API.Installers;
using Albelli.API.Middlewares;
using Albelli.Core;
using Albelli.Core.Handlers.QueryHandlers;
using Albelli.Core.PipelineBehaviors;
using Albelli.Core.Services;
using AspNetCoreRateLimit;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidation(configuration =>
{
    ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
    configuration.RegisterValidatorsFromAssemblyContaining<GetPopularMakelaarsQueryValidator>();
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
builder.Services.AddMediatR(typeof(GetOrderQueryHandler).Assembly);
builder.Services.AddScoped<IModelValidator, ModelValidator>();
builder.Services.InstallServices(builder.Configuration, builder.Environment);
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();

builder.Host.UseSerilog((context, configuration) =>
{
    var applicationName = context.Configuration.GetSection("ApplicationName").Value;
    var environment = context.HostingEnvironment.EnvironmentName;
    var elasticIndexEnvironment = "development";

    var indexFormat = $"log-{elasticIndexEnvironment}-{applicationName}-{{0:yyyy.MM.dd}}";

    var elasticSearchUri = context.Configuration.GetSection("ElasticConfiguration:Uri").Value;
    var elasticSearchSinkOptions = new ElasticsearchSinkOptions(new Uri(elasticSearchUri))
    {
        AutoRegisterTemplate = true,
        IndexFormat = indexFormat,
        BufferCleanPayload = (failingEvent, statusCode, exception) =>
        {
            dynamic e = JsonDocument.Parse(failingEvent);
            return JsonSerializer.Serialize(new Dictionary<string, object>()
            {
                { "@timestamp", e["@timestamp"] },
                { "level", "Error" },
                { "message","Error: " + e.message },
                { "messageTemplate", e.messageTemplate },
                { "failingStatusCode", statusCode },
                { "failingException", exception }
            });
        },
        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
        //EmitEventFailure = EmitEventFailureHandling.RaiseCallback,
        //FailureCallback = @event =>
        //{
        //    Console.WriteLine($"es write error -> [{@event.Exception?.GetType()?.Name}]: {@event.Exception?.Message}");
        //},
        FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                           EmitEventFailureHandling.WriteToFailureSink |
                           EmitEventFailureHandling.RaiseCallback,
        FailureSink = new FileSink("./failures.txt", new JsonFormatter(), null)
    };

    configuration
        .Enrich
        .FromLogContext()
        .WriteTo.Console()
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Elasticsearch(elasticSearchSinkOptions);
});

Serilog.Debugging.SelfLog.Enable(Console.WriteLine);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }

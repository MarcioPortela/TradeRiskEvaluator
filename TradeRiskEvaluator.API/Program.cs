using FluentValidation;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TradeRiskEvaluator.API.Infrastructure;
using TradeRiskEvaluator.Application.Common.Behaviors;
using TradeRiskEvaluator.Application.Features.CalculateRisk;
using TradeRiskEvaluator.Domain.RiskRules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IRiskRule, HighRiskRule>();
builder.Services.AddScoped<IRiskRule, MediumRiskRule>();
builder.Services.AddScoped<IRiskRule, LowRiskRule>();
builder.Services.AddScoped<RiskEvaluator>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CalculateRiskCommand).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(TradeRequestValidator).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Trade Risk Evaluator API",
        Version = "v1",
        Description = "REST API for calculating and classifying risk in financial trades. Built with Clean Architecture and MediatR.",
        Contact = new OpenApiContact
        {
            Name = "Márcio Portela",
            Email = "marcio.portela@fatec.sp.gpv.br",
            Url = new Uri("https://www.linkedin.com/in/marcio-santana-portela/")
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();

app.Run();
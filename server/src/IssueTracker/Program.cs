using Hellang.Middleware.ProblemDetails;
using IssueTracker.Application;
using IssueTracker.Infrastructure;
using IssueTracker.Infrastructure.Services.DbInitializer;
using IssueTracker.ServicesExtensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add custom services
builder.Services.AddProblemDetailsExt();
builder.Services.AddApplicationExt(builder.Configuration);
builder.Services.AddInfrastructureExt(builder.Configuration);
builder.Services.AddAuthenticationExt(builder.Configuration);

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        var enumConverter = new JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

//
// Pipline creation
//
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseProblemDetails();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

app.UseCors(opt =>
{
    opt.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins(builder.Configuration["client"]);
 
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.RunWithDataForDevelopment();
    }
}

app.Run();
public partial class Program { }


using Lockshot.Bot.API.Core.Interfaces;
using Lockshot.Bot.API.Core.Mothods;
using Lockshot.Bot.API.Data.Interfaces;
using Lockshot.Bot.API.Data.Services;
using Lockshot.Bot.API.Settings;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lockshot.Bot.API", Version = "v1" });
    c.EnableAnnotations();
});

builder.Services.Configure<MicroservicesSettings>(builder.Configuration.GetSection("MicroservicesSettings"));
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<GeneratorAnswers>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("WebApi", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lockshot.Bot.API v1");
    });
}

app.UseCors("WebApi");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

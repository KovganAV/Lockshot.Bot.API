using Lockshot.Bot.API.Core.Interfaces;
using Lockshot.Bot.API.Core.Mothods;
using Lockshot.Bot.API.Data.Interfaces;
using Lockshot.Bot.API.Data.Services;
using Lockshot.Bot.API.Settings;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.SwaggerGen;
using Lockshot.Bot.API.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IChatBotService, HuggingFaceService>(client =>
{
    client.BaseAddress = new Uri("https://api.huggingface.co/models/mistralai/Mistral-Nemo-Instruct-2407"); 
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

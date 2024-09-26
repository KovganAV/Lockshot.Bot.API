using HuggingfaceHub;
using Lockshot.Bot.API.Core.Interfaces;
using Lockshot.Bot.API.Core.Services;
using Lockshot.Bot.API.Data.Interfaces;
using Lockshot.Bot.API.Data.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "BotApi",
        policy =>
        {
            policy.WithOrigins("http://192.168.17.57:3000", "http://localhost:3000") // Локальный порт для теста 
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IChatBotService>(provider =>
{
    var apiKey = "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";
    return new HuggingFaceService(apiKey);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("BotApi");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

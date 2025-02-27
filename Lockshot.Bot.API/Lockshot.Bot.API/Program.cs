using Lockshot.Bot.API.Core.Interfaces;
using Lockshot.Bot.API.Core.Mothods;
using Lockshot.Bot.API.Core.Services;
using Lockshot.Bot.API.Settings;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MicroservicesSettings>(builder.Configuration.GetSection("MicroservicesSettings"));

builder.Services.AddScoped<IBotsService, BotsService>();

builder.Services.AddScoped<GeneratorAnswers>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "WebApi",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "https://localhost:7044")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("WebApi");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

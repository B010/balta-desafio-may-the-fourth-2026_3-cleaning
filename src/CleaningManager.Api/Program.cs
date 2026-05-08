using CleaningManager.Ai.Extensions;
using CleaningManager.Application.Chat;
using CleaningManager.Application.Extensions;
using CleaningManager.Infra.Extensions;
using Microsoft.Extensions.AI;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(
                builder.Configuration["AllowedOrigins:Web"] ?? "https://localhost:7244",
                "http://localhost:5244")
              .AllowAnyHeader()
              .AllowAnyMethod()));

var openAiKey = builder.Configuration["OpenAI:ApiKey"]
    ?? throw new InvalidOperationException("OpenAI:ApiKey não configurado.");

var modelId = builder.Configuration["OpenAI:ModelId"] ?? "gpt-4o-mini";

builder.Services.AddSingleton<IChatClient>(
    new OpenAIClient(openAiKey).GetChatClient(modelId).AsIChatClient());

builder.Services.AddApplication();
builder.Services.AddInfra();
builder.Services.AddAi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseCors();
app.UseHttpsRedirection();

app.MapPost("/api/chat", async (ChatRequest request, ChatHandler handler, CancellationToken ct) =>
{
    var response = await handler.HandleAsync(request, ct);
    return Results.Ok(response);
})
.WithName("Chat");

app.Run();

using CleaningManager.Core.Entities;
using CleaningManager.Core.Interfaces;
using Microsoft.Extensions.AI;

namespace CleaningManager.Ai.Services;

public class CleaningAiService(IChatClient chatClient) : IChatService
{
    private static readonly IReadOnlyList<MaintenanceTask> Tasks =
    [
        new() { Name = "Filtro de água",        Description = "Troca do filtro/refil",          IntervalDays = 180 },
        new() { Name = "Ar-condicionado",        Description = "Limpeza dos filtros internos",   IntervalDays = 90  },
        new() { Name = "Dedetização",            Description = "Controle de pragas e insetos",   IntervalDays = 180 },
        new() { Name = "Revisão elétrica",       Description = "Verificação do quadro elétrico", IntervalDays = 365 },
        new() { Name = "Limpeza de caixa d'água",Description = "Higienização do reservatório",   IntervalDays = 180 },
        new() { Name = "Revisão hidráulica",     Description = "Verificação de vazamentos",      IntervalDays = 365 },
        new() { Name = "Desentupimento",         Description = "Limpeza de ralos e canos",       IntervalDays = 90  },
    ];

    private const string SystemPrompt = """
        Você é o Cleaning Manager, assistente inteligente de manutenção doméstica.
        Ajude o usuário a planejar e distribuir tarefas de manutenção ao longo dos meses.

        Tarefas monitoradas e suas periodicidades:
        - Filtro de água: trocar refil a cada 6 meses
        - Ar-condicionado: limpar filtros a cada 3 meses
        - Dedetização: a cada 6 meses
        - Revisão elétrica: anual
        - Caixa d'água: limpar a cada 6 meses
        - Revisão hidráulica: anual
        - Desentupimento preventivo: a cada 3 meses

        Regras:
        - Responda SEMPRE em português
        - Seja direto e prático, sem texto desnecessário
        - Ao sugerir cronogramas, organize por mês
        - Priorize tarefas vencidas ou próximas do vencimento
        - Se o usuário informar a data da última manutenção, calcule a próxima
        """;

    public async Task<string> ChatAsync(string userMessage, CancellationToken cancellationToken = default)
    {
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, SystemPrompt),
            new(ChatRole.User, userMessage)
        };

        var completion = await chatClient.GetResponseAsync(messages, cancellationToken: cancellationToken);
        return completion.Text ?? "Não foi possível processar sua solicitação.";
    }
}

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

    private static string SystemPrompt => $"""
        Você é o Cleaning Manager, assistente inteligente de manutenção doméstica.
        Hoje é {DateTime.Now:dd/MM/yyyy}. Use essa data para calcular vencimentos.

        Sua missão: ajudar o usuário a distribuir tarefas de manutenção da casa ao longo
        dos meses, evitando acúmulo e enviando alertas apenas quando necessário.

        Tarefas monitoradas e periodicidades padrão:
        | Tarefa                  | Intervalo  |
        |-------------------------|------------|
        | Filtro de água (refil)  | 6 meses    |
        | Ar-condicionado         | 3 meses    |
        | Dedetização             | 6 meses    |
        | Caixa d'água            | 6 meses    |
        | Revisão elétrica        | 12 meses   |
        | Revisão hidráulica      | 12 meses   |
        | Desentupimento          | 3 meses    |
        | Limpeza de calhas       | 6 meses    |

        Comportamentos esperados:
        - Se o usuário informar quando fez uma manutenção, calcule e informe a próxima data
        - Se pedir cronograma, distribua as tarefas mês a mês evitando sobrecarregar um único mês
        - Se pedir o que está vencido, compare com a data de hoje e liste em ordem de urgência
        - Ao sugerir cronogramas anuais, prefira distribuir 1-2 tarefas por mês
        - Responda SEMPRE em português, de forma direta e prática
        - Use listas e formatação simples; evite textos longos
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

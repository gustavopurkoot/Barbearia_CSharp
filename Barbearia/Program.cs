using Barbearia.Models;
using Barbearia.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDataContext>(options =>
options.UseSqlite("Data Source=Barbearia.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/Servicos", (AppDataContext context) =>
{
    return context.Servicos;
});

app.MapGet("/Agendamentos", (AppDataContext context) =>
{
    return context.Agendamentos.Include(agendamento => agendamento.Servico);
});

app.MapGet("/servicos/{id}", (int id, AppDataContext context) =>
{
    var servico = context.Servicos.Find(id);
    if (servico is null) return Results.NotFound();
    return Results.Ok(servico);
});

app.MapGet("/agendamentos/{id}", (int id, AppDataContext context) =>
{
    var agendamento = context.Agendamentos.Include(a => a.Servico).FirstOrDefault(a => a.Id == id);
    if (agendamento is null) return Results.NotFound();
    return Results.Ok(agendamento);
});

app.MapPost("/Servicos", (Servico servico, AppDataContext context) =>
{
    if (servico.preco <= 0)
    {
        return Results.BadRequest("O preço deve ser maior que zero.");
    }
    context.Servicos.Add(servico);
    context.SaveChanges();
    return Results.Created($"/servicos/{servico.Id}",
     servico);
});

app.MapPost("/Agendamentos", (Agendamento novoAgendamento, AppDataContext context) =>
{
    var servicoNovo = context.Servicos.Find(novoAgendamento.ServicoId);

    if (servicoNovo is null)
    {
        return Results.BadRequest("ServicoId inválido");
    }

    if (novoAgendamento.dataHora < DateTime.Now)
    {
        return Results.BadRequest("A data e hora do agendamento devem ser futuras.");
    }

    foreach (var agendamentoExistente in context.Agendamentos.Include(a => a.Servico))
    {
        if (novoAgendamento.dataHora < agendamentoExistente.dataHora.AddMinutes(agendamentoExistente.Servico.duracaoMinutos) &&
            novoAgendamento.dataHora.AddMinutes(servicoNovo.duracaoMinutos) > agendamentoExistente.dataHora)
        {
            return Results.BadRequest("Já existe um agendamento nesse horário.");
        }
    }

    novoAgendamento.Servico = servicoNovo;

    context.Agendamentos.Add(novoAgendamento);
    context.SaveChanges();

    return Results.Created($"/agendamentos/{novoAgendamento.Id}", novoAgendamento);
});

app.MapPut("/servicos/{id}", (int id, Servico novo, AppDataContext context) =>
{
    var servico = context.Servicos.Find(id);
    if (servico is null)
    {
        return Results.NotFound();
    }
    else
    {
        servico.nome = novo.nome;
        servico.preco = novo.preco;
        servico.duracaoMinutos = novo.duracaoMinutos;
        context.SaveChanges();
        return Results.Ok(servico);
    }
});

app.MapPut("/agendamentos/{id}", (int id, Agendamento novo, AppDataContext context) =>
{
    var agendamento = context.Agendamentos.Find(id);
    if (agendamento is null)
    {
        return Results.NotFound();
    }
    else
    {
        agendamento.clienteNome = novo.clienteNome;
        agendamento.dataHora = novo.dataHora;
        agendamento.status = novo.status;
        agendamento.Servico = context.Servicos.Find(novo.ServicoId);
        if (agendamento.Servico is null)
        {
            return Results.BadRequest("ServicoId inválido");
        }

        context.SaveChanges();
        return Results.Ok(agendamento);
    }
});

app.MapDelete("/servicos/{id}", (int id, AppDataContext context) =>
{
    var servico = context.Servicos.Find(id);
    if (servico is null)
    {
        return Results.NotFound();
    }
    else
    {
        context.Servicos.Remove(servico);
        context.SaveChanges();
        return Results.Ok(servico);
    }
});

app.MapDelete("/agendamentos/{id}", (int id, AppDataContext context) =>
{
    var agendamento = context.Agendamentos.Find(id);
    if (agendamento is null)
    {
        return Results.NotFound();
    }
    else
    {
        context.Agendamentos.Remove(agendamento);
        context.SaveChanges();
        return Results.Ok(agendamento);
    }
});

app.Run();

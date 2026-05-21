using Barbearia.Models;
using Barbearia.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDataContext>(options =>
options.UseSqlite("Data Source=produtos.db"));

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
    return context.Agendamentos;
});

app.MapGet("/servicos/{id}", (int id, AppDataContext context) =>
{
    var servico = context.Servicos.Find(id);
    if (servico is null) return Results.NotFound();
    return Results.Ok(servico);
});

app.MapGet("/agendamentos/{id}", (int id, AppDataContext context) =>
{
    var agendamento = context.Agendamentos.Find(id);
    if (agendamento is null) return Results.NotFound();
    return Results.Ok(agendamento);
});

app.MapPost("/Servicos", (Servico servico, AppDataContext context) =>
{
    context.Servicos.Add(servico);
    context.SaveChanges();
    return Results.Created($"/servicos/{servico.Id}",
     servico);
});

app.MapPost("/Agendamentos", (Agendamento agendamento, AppDataContext context) =>
{
    context.Agendamentos.Add(agendamento);
    context.SaveChanges();
    return Results.Created($"/agendamentos/{agendamento.Id}",
     agendamento);
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
        agendamento.servico = context.Servicos.Find(novo.ServicoId);
        if (agendamento.servico is null)
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

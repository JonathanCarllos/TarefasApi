using Dapper.Contrib.Extensions;
using TarefasApi.Data;
using static TarefasApi.Data.TarefaContext;

namespace TarefasApi.Endpoints;
public static class TarefasEndpoints
{
    public static void MapTarefasEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => $"Bem-vindo a API Tarefas {DateTime.Now}");

        app.MapGet("/Tarefas", async (GetConnection connectionGetter) =>
        {
            using var con = await connectionGetter();
            var tarefas = con.GetAll<Tarefa>().ToList();
            if (tarefas is null)
                return Results.NotFound();
            return Results.Ok(tarefas);           
        });

        app.MapGet("/Tarefas/{id}", async (GetConnection connectionGetter, int id) =>
        {
            using var con = await connectionGetter();
            var tarefas = con.Get<Tarefa>(id);
            /*if (tarefas is null)
                return Results.NotFound();
            return Results.Ok(tarefas);*/
            return con.Get<Tarefa>(id) is Tarefa tarefa ? Results.Ok(tarefa) : Results.NotFound();
        });
        app.MapPost("/tarefa", async (GetConnection connectionGetter, Tarefa tarefa) =>
        {
            var con = await connectionGetter();
            var id =con.Insert(tarefa);
            return Results.Created($"/tarefa{id}",tarefa);
        });
        app.MapPut("/tarefa", async (GetConnection connectionGetter, Tarefa tarefa) =>
        {
            var con = await connectionGetter();
            var id = con.Update(tarefa);
            return Results.Created($"/tarefa{id}", tarefa);
        });
        app.MapDelete("/Tarefas/{id}", async (GetConnection connectionGetter, int id) =>
        {
            using var con = await connectionGetter();
            var deleted = con.Get<Tarefa>(id);
            if (deleted is null)
                return Results.NotFound();
            con.Delete(deleted);
            return Results.Ok(deleted);
        });
    }
}

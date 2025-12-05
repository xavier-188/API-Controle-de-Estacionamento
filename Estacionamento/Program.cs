using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddCors(options =>
    options.AddPolicy("Acesso Total",
        configs => configs
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod())
);

var app = builder.Build();

app.MapGet("/", () => "Estacionamento!");

app.MapPost("/api/estacionamento/registrar" , ([FromServices] AppDbContext ctx, [FromBody] Carro carro) =>
{
    Carro? resultado =
    ctx.Carros.FirstOrDefault(p => p.Placa == carro.Placa);
    if (resultado is not null)
    {
        return Results.Conflict("Carro já cadastrado!");
    }

    ctx.Carros.Add(carro);
    ctx.SaveChanges();
    return Results.Created("", carro);
    
});

app.MapGet("/api/estacionamento/listar", ([FromServices] AppDbContext ctx) =>
{
    if (ctx.Carros.Any())
    {
        return Results.Ok(ctx.Carros.ToList());
    }
    return Results.NotFound("Lista Vazia");
});

app.MapPut("api/estacionamento/{id}/saida", ([FromServices] AppDbContext ctx, [FromRoute] string id ) =>
{
     Carro? carroBuscado = ctx.Carros.Find(id);

    if (carroBuscado == null)
    {
        return Results.NotFound("Carro não encontrado");
    }

    carroBuscado.Saida = DateTime.Now;
    carroBuscado.Status = "Saiu";
    ctx.SaveChanges();
    return Results.Ok(carroBuscado);
});

app.MapDelete("/api/estacionamento/remover/{id}", ([FromServices] AppDbContext ctx, [FromRoute] string id) =>
{
    Carro? carroBuscado = ctx.Carros.Find(id);

    if (carroBuscado == null)
    {
        return Results.NotFound("Carro não encontrado");
    }

    ctx.Carros.Remove(carroBuscado);
    ctx.SaveChanges();
    return Results.Ok("Carro Removido!");
});

app.MapGet("/api/estacionamento/estacionados", ([FromServices] AppDbContext ctx) =>
{
    return Results.Ok(ctx.Carros.Where(x => x.Status == "Estacionado"));
});

app.MapGet("/api/estacionamento/buscar/{placa}", ([FromServices] AppDbContext ctx, [FromRoute] string placa) =>
{
    Carro carroBuscado = ctx.Carros.FirstOrDefault(c => c.Placa == placa);

    if (carroBuscado == null)
    {
        return Results.NotFound("Carro não encontrado");
    }
    return Results.Ok(carroBuscado);

});

app.Run();

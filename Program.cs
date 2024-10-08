using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Models;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Pizzas") ?? "Data Source=Pizzas.db";



builder.Services.AddEndpointsApiExplorer();

//These calls make it so we use an internal, in memory db
// builder.Services.AddDbContext<PizzaDb>(options => options.UseInMemoryDatabase("items"));
// builder.Services.AddDbContext<StoreDb>(options => options.UseInMemoryDatabase("items"));

//Establish a DB in SQLite
builder.Services.AddSqlite<PizzaDb>(connectionString);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "PizzaStore aPI",
        Description = "MAking the Pizzas you love",
        Version= "v1"
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.ljson", "PizzaStore API V1");
    });
}

//With entity framwork, this is VERY lightweight way of just doing standard crud stuff. 
//It's all async and has to be due ot EntityFramewookr stuff
//The general Idea of all this is that you use an async function and call a series of async methods
//in these app.map methods.   
app.MapGet("/", () => "Hello World!");
app.MapGet("/pizzas", async (PizzaDb db) => await db.Pizzas.ToListAsync());
app.MapGet("/pizza/{id}", async (PizzaDb db, int id) => await db.Pizzas.FindAsync(id));


//In the following methods, you changing data on the db. For this, you do all the changes to the
//object, then you have to run await db.SaveChangesAsyc();
app.MapPost("/pizza", async (PizzaDb db, Pizza pizza) =>
{
    await db.Pizzas.AddAsync(pizza);
    await db.SaveChangesAsync();
    return Results.Created($"/pizza/{pizza.Id}", pizza);
});

app.MapPut("/pizza/{id}", async (PizzaDb db, Pizza updatePizza, int id) => 
{
    var pizza = await db.Pizzas.FindAsync(id);
    if(pizza is null) return Results.NotFound();
    pizza.Name = updatePizza.Name;
    pizza.Description = updatePizza.Description;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/pizza/{id}", async (PizzaDb db, int id) => {
    var pizza = await db.Pizzas.FindAsync(id);
    if(pizza is null) return Results.NotFound();
    db.Pizzas.Remove(pizza);
    await db.SaveChangesAsync();
    return Results.Ok();
});


//This is the problem line.
// app.MapGet("/stores", async (StoreDb db) => await db.Stores.ToListAsync());

app.Run();

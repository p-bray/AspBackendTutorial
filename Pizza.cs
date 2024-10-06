using Microsoft.EntityFrameworkCore;

namespace PizzaStore.Models
{
    public class Pizza
    {
        public int Id {get; set;}
        public string? Name {get; set;}
        public string? Description {get; set;}
    }

    //using the entity framework, we need to create a context to help create the database connection. 
    public class PizzaDb : DbContext
    {
        public PizzaDb(DbContextOptions options) : base(options) {}
        public DbSet<Pizza> Pizzas {get; set;} = null!;
    }
}
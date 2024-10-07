using Microsoft.EntityFrameworkCore;

namespace PizzaStore.Models
{
    public class Store  
    {
        public int Id;
        public string? Name {get; set;}
        public string? Address {get; set;}
        public string? Owner {get; set;}
    }

    //This DbContext essentially just let's you have a Database session.
    public class StoreDb : DbContext 
    {
        //contstructor that passes any custom options
        public StoreDb(DbContextOptions options) : base(options) {}
        //DbSet stores essentially just represents the Store entities in the Db
        public DbSet<Store> Stores {get; set;} = null!;
    }
}



using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{

    public DbSet<Carro> Carros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Estacionamento.db");
    }
}



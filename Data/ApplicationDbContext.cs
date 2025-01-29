using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GAMER.Models;

namespace GAMER.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GAMER.Models.Desarrollador> Desarrollador { get; set; } = default!;
        public DbSet<GAMER.Models.Genero> Genero { get; set; } = default!;
        public DbSet<GAMER.Models.Plataforma> Plataforma { get; set; } = default!;
        public DbSet<GAMER.Models.Videojuego> Videojuego { get; set; } = default!;
        public DbSet<GAMER.Models.VideojuegoPlataforma> VideojuegoPlataformas { get; set; } = default!;
    }
}

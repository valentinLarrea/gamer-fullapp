namespace GAMER.Models
{
    public class Videojuego
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public string? Sinopsis { get; set; }
        public string? Imagen { get; set; }
        public int GeneroId { get; set; }
        public Genero? Genero { get; set; }
        public int DesarrolladorId { get; set; }
        public Desarrollador? Desarrollador { get; set; }
        public ICollection<VideojuegoPlataforma> VideojuegoPlataformas { get; set; } = new List<VideojuegoPlataforma>();

    }
}

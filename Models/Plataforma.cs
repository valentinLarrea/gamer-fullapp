namespace GAMER.Models
{
    public class Plataforma
    {
        public int Id { get; set; }
        public required string Tipo { get; set; }
        public ICollection<VideojuegoPlataforma> VideojuegoPlataformas { get; set; } = new List<VideojuegoPlataforma>();
    }
}

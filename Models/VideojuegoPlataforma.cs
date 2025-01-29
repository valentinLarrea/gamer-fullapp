namespace GAMER.Models
{
    public class VideojuegoPlataforma
    {
        public int Id { get; set; }
        public int VideojuegoId { get; set; }
        public Videojuego? Videojuego { get; set; }
        public int PlataformaId { get; set; }
        public Plataforma? Plataforma { get; set; }
    }
}

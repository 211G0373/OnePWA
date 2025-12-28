namespace OnePWA.Models.DTOs
{
    public class PlayerDTO : IPlayerDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int CardsCount { get; set; }
        public string FotoPerfil { get; set; }

    }
}

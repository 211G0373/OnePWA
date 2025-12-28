namespace OnePWA.Models.DTOs
{
    public interface IPlayerDTO
    {
        int Id { get; set; }
        string UserName { get; set; }
        int CardsCount { get; set; }

       string FotoPerfil { get; set; }


    }
}

namespace OnePWA.Models.DTOs
{
    //esto es para mandar todas las cartas de una 
    public interface ICardDTO
    {
        int Id { get; set; }

        string Name { get; set; }

        string Email { get; set; }

        int WonGames { get; set; }

        string Password { get; set; }
    }

}
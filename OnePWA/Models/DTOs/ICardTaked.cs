namespace OnePWA.Models.DTOs
{
    public interface ICardTaked
    {
        CardDTO Card { get; set; }
        int IdTurn { get; set; }
    }
}

namespace OnePWA.Models.DTOs
{
    public class TakedCard : ITakedCardDTO
    {
        public int IdPlayer { get ; set ; }
        public int IdTurn { get ; set ; }
        public int Time { get; set; }

    }
}

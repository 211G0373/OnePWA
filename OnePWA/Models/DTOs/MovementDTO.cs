namespace OnePWA.Models.DTOs
{
    public class MovementDTO : IMovementDTO
    {
        public int IdPlayer { get ; set ; }

        public int Time { get; set; }

        public bool Reverse { get ; set ; }
        public int IdTurn { get ; set ; }



        public CardDTO Card { get; set; }


    }
}

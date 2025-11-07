namespace OnePWA.Models.DTOs
{


    //Signalr
    public interface IMovementDTO
    {
        int IdPlayer { get; set; }
        int IdCard { get; set; }
        int IdTurn { get; set; }
    }

}

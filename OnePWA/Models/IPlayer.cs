namespace OnePWA.Models
{
    public interface IPlayer
    {
        int Id { get; set; }
        string Username { get; set; }
        int SignalrId { get; set; }
        byte turnOrder { get; set; }
        List<int> Cards { get; set; }

    }
}

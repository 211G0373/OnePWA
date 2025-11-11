using OnePWA.Models.Entities;

namespace OnePWA.Models
{
    public interface IPlayer
    {
        int Id { get; set; }
        string Username { get; set; }
        byte turnOrder { get; set; }
        List<Cards> Cards { get; set; }

    }
}

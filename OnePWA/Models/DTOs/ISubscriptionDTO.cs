namespace OnePWA.Models.DTOs
{
    public class ISubscriptionDTO
    {
        public string Endpoint { get; set; } = null!;
        public Keys Keys { get; set; } = null!;
    }

    public class Keys
    {
        public string P256dh { get; set; } = null!;
        public string Auth { get; set; } = null!;
    }
}

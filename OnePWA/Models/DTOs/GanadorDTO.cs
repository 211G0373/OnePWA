namespace OnePWA.Models.DTOs
{
    public class GanadorDTO
    {
        public int Ganador { set; get; }
        public List<int> players { get; set; } = new();
    }
}

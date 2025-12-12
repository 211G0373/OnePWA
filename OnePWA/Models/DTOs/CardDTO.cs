namespace OnePWA.Models.DTOs
{
    public class CardDTO:ICardDTO
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
    }
}

namespace OnePWA.Models.DTOs
{
    public interface ICreateSesionDTO
    {
        string Name { get; set; }
        bool Private { get; set; }
        bool NewRules { get; set; }

    }
}

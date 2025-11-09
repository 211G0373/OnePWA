namespace OnePWA.Models.DTOs
{
    public class CreateSessionDTO : ICreateSesionDTO
    {
        public string Name { get ; set ; }
        public bool Private { get ; set ; }
        public bool NewRules { get ; set ; }
    }
}

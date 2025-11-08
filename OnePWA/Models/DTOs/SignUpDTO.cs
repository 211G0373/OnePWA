namespace OnePWA.Models.DTOs
{
    public class SignUpDTO : ISignUpDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

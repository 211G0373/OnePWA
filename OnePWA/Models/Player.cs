using OnePWA.Models.Entities;

namespace OnePWA.Models
{
    public class Player : IPlayer
    {
        public int Id { get ; set ; }
        //public string Username { get ; set ; } 
        public byte turnOrder { get ; set ; }
        public List<Cards> Cards { get ; set ; }= new List<Cards>();
    }
}

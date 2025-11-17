
using OnePWA.Models.DTOs;
using OnePWA.Models.Entities;
using OnePWA.Services;
using System.Timers;

namespace OnePWA.Models
{
    public class GameSession : IGameSesion
    {
        public int Id { get ; set ; }
        public string Name { get ; set ; }
        public string Code { get ; set ; }
        public int IdHost { get ; set ; }
        public LinkedList<IPlayer> Players { get ; set ; } = new LinkedList<IPlayer>();
        public bool Started { get ; set ; }
        public bool Private { get ; set ; }
        public bool NewRules { get ; set ; }
        public Cards TopCard { get; set; } = new Cards();
        public string LastColor { get ; set ; }
        public System.Timers.Timer Timer { get ; set ; } = new System.Timers.Timer();
        public int IdTurn { get ; set ; }
        public List<int> UsedCards { get ; set ; } = new List<int>();
        public List<int> NotUsed { get ; set ; } = new List<int>();
        public List<Cards> Cards { get ; set ; }= new List<Cards>();
        public SignalrService Notifications { get; }

        public GameSession(SignalrService notifications)
        {

           
            Notifications = notifications;
        }


        public void PlayCard(int idPlayer, int card)
        {
            var p= Players.FirstOrDefault(pl => pl.Id == idPlayer);

            if(!p.Cards.Any(c => c.Id == card))
            {
               throw new Exception("El jugador no tiene esa carta");
            }
            var c= Cards.First(c => c.Id == card);
           

            if (c.Color == "black")
            {
                TopCard = c;
                LastColor = c.Color;

            }
            else if(c.Color == LastColor || c.Name == TopCard.Name)
            {
                TopCard = c;
                LastColor = c.Color;
                if (c.Name == "Skip")
                {
                    SkipTurn();
                    NextTurn();

                }
                if (c.Name == "Reverse")
                {
                    ReverseTurn();
                    NextTurn();
                }
            }
            else
            {
                throw new Exception("Movimiento invalido");
            }
            p.Cards.Remove(c);
            UsedCards.Add(card);




            if (p == null)
                return;

            if (p.Id == IdTurn)
            {
                //Timer.Stop();
            }

            
        }


        public void ChangeColor(int idPlayer, ChangeColorDTO dto)
        {
            var p = Players.FirstOrDefault(pl => pl.Id == idPlayer);
            if (!p.Cards.Any(c => c.Id == dto.IdCard))
            {
                throw new Exception("El jugador no tiene esa carta");
            }
           
            if (Cards.FirstOrDefault(x=>x.Id==dto.IdCard).Name=="Wild")
            {
                var c = Cards.First(c => c.Id == dto.IdCard);
                p.Cards.Remove(c);
                UsedCards.Add(dto.IdCard);
                TopCard = c;
                LastColor = dto.Color;
            }
            else
            {
                throw new Exception("Movimiento invalido");
            }
            if (p == null)
                return;
            if (p.Id == IdTurn)
            {
                Timer.Stop();
            }
        }




        public void NextTurn()
        {
            var node = Players.First;

            // Buscar el nodo que coincide con el jugador actual
            while (node != null && node.Value.Id != IdTurn)
            {
                node = node.Next;
            }

            if (node != null)
            {
                var nextNode = node.Next ?? Players.First;
                IdTurn = nextNode.Value.Id;
                //Timer.Start();
            }

            //notificar
        }

        public void ResetGame()
        {
        }

        public void ReverseTurn()
        {
            Players.Reverse();
        }

        public void SkipTurn()
        {
            var node = Players.First;

            // Buscar el nodo que coincide con el jugador actual
            while (node != null && node.Value.Id != IdTurn)
            {
                node = node.Next;
            }

            if (node != null)
            {
                var nextNode = node.Next ?? Players.First;
                IdTurn = nextNode.Value.Id;
            }
        }

        public void StartGame()
        {
            //ordenar aleatoriamente los jugadores
            var rnd = new Random();
            Players = new LinkedList<IPlayer>(Players.OrderBy(x => rnd.Next()));
            IdTurn = Players.First.Value.Id;
            //ordenar cartas aleatoriamente
            NotUsed = Cards.Select(c => c.Id).OrderBy(x => rnd.Next()).ToList();
            Started = true;
            //repartir cartas a los jugadores
            foreach (var player in Players)
            {
                for (int i = 0; i < 7; i++)
                {
                    var cardId = NotUsed[0];
                    player.Cards.Add(Cards.First(c => c.Id == cardId));
                    NotUsed.RemoveAt(0);
                }
            }
            Timer.Elapsed += playerOut;
            Timer.AutoReset = false;
            Timer.Interval = 10000; // 60 segundos
           // Timer.Start();


        }

        public void StopGame()
        {
            throw new NotImplementedException();
        }

        public async void playerOut(object? sender, ElapsedEventArgs e)
        {
            /// eliminar jugador por inactividad
            
            ///notificar a los demas jugadores
            ///si es su turno, pasar al siguiente
            ///
            foreach(var p in Players)
            {
                await Notifications.PlayerLeft(p.Id.ToString(),IdTurn);

            }

            NextTurn();
        }

    }
}

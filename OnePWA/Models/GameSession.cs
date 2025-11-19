
using OnePWA.Models.DTOs;
using OnePWA.Models.Entities;
using OnePWA.Services;
using System.Threading.Tasks;
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


        public async Task PlayCard(int idPlayer, int card)
        {
            var p = Players.FirstOrDefault(pl => pl.Id == idPlayer);

            if (!p.Cards.Any(c => c.Id == card))
            {
                throw new Exception("El jugador no tiene esa carta");
            }
            var c = Cards.First(c => c.Id == card);


            if (c.Color == "black")
            {
                throw new Exception("Necesitas elegir un color");

            }
            else if (c.Color == LastColor || c.Name == TopCard.Name)
            {
                TopCard = c;
                LastColor = c.Color;
                if (c.Name == "Skip")
                {
                    SkipTurn();
                  

                }
                if (c.Name == "Reverse")
                {
                    ReverseTurn();
                  
                }
            }
            else
            {
                throw new Exception("Movimiento invalido");
            }
            p.Cards.Remove(c);
            UsedCards.Add(card);

            if (p.Id == IdTurn)
            {
                //Timer.Stop();
            }

            NextTurn();
            

            foreach (var player in Players)
            {
                if (player.Id == idPlayer)
                {
                    continue;
                }
                await Notifications.PlayerColocoCard(player.Id.ToString(), new MovementDTO
                {
                    IdPlayer = idPlayer,
                    Card = new CardDTO
                    {
                        Id = c.Id,
                        Color = c.Color,
                    },
                    IdTurn = IdTurn
                });



            }

            var currentPlayer = Players.FirstOrDefault(pl => pl.Id == IdTurn);
            if (currentPlayer != null)
            {
                if (!currentPlayer.Cards.Any(x => x.Color == "black" || x.Color == LastColor || x.Name == TopCard.Name))
                {
                    //el siguiente jugador no tiene cartas validas, toma una carta
                    ///aqui
                    ///
                    int tcard =TakeCard(currentPlayer.Id);
                    if (!currentPlayer.Cards.Any(x => x.Color == "black" || x.Color == LastColor || x.Name == TopCard.Name))
                    {
                        //si aun asi no tiene cartas validas, saltar su turno
                        NextTurn();
                        //notificar de carta tomada y turno saltado
                       // Notifications.PlayerTakeCard(ITakedCardDTO);

                    }
                    foreach (var player in Players)
                    {
                        if (player.Id == idPlayer)
                        {
                            await Notifications.YouTakeCard(player.Id.ToString(), new CardTaked()
                            {
                                IdTurn = IdTurn,
                                Card = new CardDTO
                                {
                                    Id = tcard,
                                    Color = Cards.First(ca => ca.Id == tcard).Color
                                }
                            });
                            continue;
                        }
                        await Notifications.PlayerTakeCard(player.Id.ToString(), new TakedCard() 
                        { 
                            IdPlayer=currentPlayer.Id,
                            IdTurn=IdTurn
                        });
                    }
                }
            }

        }

        public int TakeCard(int idPlayer)
        {
            var p = Players.FirstOrDefault(pl => pl.Id == idPlayer);
            if (p == null)
            {
                throw new Exception("Jugador no encontrado");
            }
            if (NotUsed.Count == 0)
            {
                //revolver usadas
                NotUsed = UsedCards;
                UsedCards = new List<int>();
            }
            var cardId = NotUsed[0];
            p.Cards.Add(Cards.First(c => c.Id == cardId));
            NotUsed.RemoveAt(0);
            return cardId;

            //notificar

        }


        public async Task BlackCard(int idPlayer, ChangeColorDTO dto)
        {
            var p = Players.FirstOrDefault(pl => pl.Id == idPlayer);

            if( p == null)
            {
                throw new Exception("Jugador no encontrado");
            }

            if (!p.Cards.Any(c => c.Id == dto.IdCard))
            {
                throw new Exception("El jugador no tiene esa carta");
            }
            var c = Cards.First(c => c.Id == dto.IdCard);


            if (c.Color != "black")
            {
                throw new Exception("Esta carta no permite cambio de color");

            }
            else if (c.Name == "Wild +4")
            {
                TopCard = c;
                LastColor = dto.Color;

                //SkipTurn();
                //NextTurn();

                //ReverseTurn();
                //NextTurn();

            }
            else
            {
                TopCard = c;
                LastColor = dto.Color;

                /////////throw new Exception("Movimiento invalido");
            }
            p.Cards.Remove(c);
            UsedCards.Add(dto.IdCard);

            NextTurn();
            foreach (var player in Players)
            {
                if (player.Id == idPlayer)
                {
                    continue;
                }
                await Notifications.PlayerColocoCard(player.Id.ToString(), new MovementDTO
                {
                    IdPlayer = idPlayer,
                    Card = new CardDTO
                    {
                        Id = c.Id,
                        Color = c.Color,
                    },
                    IdTurn = IdTurn
                });
            }
            var currentPlayer = Players.FirstOrDefault(pl => pl.Id == IdTurn);
            if (currentPlayer != null)
            {
                if (!currentPlayer.Cards.Any(x => x.Color == "black" || x.Color == LastColor || x.Name == TopCard.Name))
                {
                    //el siguiente jugador no tiene cartas validas, toma una carta
                    ///aqui
                    ///
                    int tcard = TakeCard(currentPlayer.Id);
                    if (!currentPlayer.Cards.Any(x => x.Color == "black" || x.Color == LastColor || x.Name == TopCard.Name))
                    {
                        //si aun asi no tiene cartas validas, saltar su turno
                        NextTurn();
                        //notificar de carta tomada y turno saltado
                        // Notifications.PlayerTakeCard(ITakedCardDTO);

                    }
                }
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
            var firstcard = NotUsed.FirstOrDefault();
            UsedCards.Add(firstcard);
            NotUsed.Remove(firstcard);

            TopCard = Cards.FirstOrDefault(x=>x.Id==UsedCards.First());
            LastColor = TopCard.Color; 

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

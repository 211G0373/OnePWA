
using Microsoft.EntityFrameworkCore;
using OnePWA.Models.DTOs;
using OnePWA.Models.Entities;
using OnePWA.Services;
using System.Threading.Tasks;
using System.Timers;

namespace OnePWA.Models
{
    public class GameSession : IGameSesion
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsReversed { get; set; } = false;
        public string Code { get; set; }
        public int IdHost { get; set; }
        public int AcumulatedCards { get; set; }
        public LinkedList<IPlayer> Players { get; set; } = new LinkedList<IPlayer>();
        public bool Started { get; set; }
        public bool Private { get; set; }
        public bool NewRules { get; set; }
        public Cards TopCard { get; set; } = new Cards();
        public string LastColor { get; set; }
        public System.Timers.Timer Timer { get; set; } = new System.Timers.Timer();
        public System.Timers.Timer AutoStartTimer { get; set; } = new System.Timers.Timer();
        public System.Timers.Timer ReplayTimer { get; set; } = new System.Timers.Timer();
        public DateTime PlayerstartTime { get; set; }
        public DateTime ReplaystarTime { get; set; }
        public DateTime AutostartTime { get; set; }


        public int IdTurn { get; set; }
        public List<int> UsedCards { get; set; } = new List<int>();
        public List<int> NotUsed { get; set; } = new List<int>();
        public List<Cards> Cards { get; set; } = new List<Cards>();
        public SignalrService Notifications { get; }

        private readonly ISessionContext _context;

        public GameSession(SignalrService notifications, ISessionContext context)
        {
            Notifications = notifications;
            _context = context;


            AutoStartTimer.Elapsed += autoStart;
            AutoStartTimer.AutoReset = false;
            AutoStartTimer.Interval = 30000; // 60 segundos

            ReplayTimer.Elapsed += Close;
            ReplayTimer.AutoReset = false;
            ReplayTimer.Interval = 10000; // 60 segundos

            AutostartTime = DateTime.Now;
            AutoStartTimer.Start();
            

        }

        private void Close(object? sender, ElapsedEventArgs e)
        {
            Close();
        }

        private void autoStart(object? sender, ElapsedEventArgs e)
        {
            if (Players.Count < 2 && !Started)
            {
                Close();
                return;
            }
            StartGame();
        }



        public void Close()
        {
            _context.Remove(this);
        }
        //jugar carta normal
        public async Task PlayCard(int idPlayer, int card)
        {
            //verificar carta (refactorizar despues)
            var p = Players.FirstOrDefault(pl => pl.Id == idPlayer);




            if (!p.Cards.Any(c => c.Id == card))
            {
                throw new Exception("El jugador no tiene esa carta");
            }
            var c = Cards.First(c => c.Id == card);

            if(!(c.Color == LastColor || c.Name == TopCard.Name))
            {
                throw new Exception("Necesitas elegir un color");

            }


            if (c.Color == "black")
            {
                throw new Exception("Necesitas elegir un color");

            }
            else if (c.Color == LastColor || c.Name == TopCard.Name)
            {

                TopCard = c;
                LastColor = c.Color;
                p.Cards.Remove(c);
                UsedCards.Add(card);


                if (p.Id == IdTurn)
                {
                    Timer.Stop();
                }




                if (c.Name == "+2")
                {
                    SkipTurn();
                    if (!NewRules)
                    {
                        _= NotifyPlayerPlaceCard(idPlayer, -1, new CardDTO()
                        {
                            Color = c.Color,
                            Id = c.Id,
                            Name = c.Name
                        });
                        if (p.Cards.Count == 0)
                        {

                            _= GameFinished(idPlayer);

                            return;
                        }

                        _ = TakeXCards(IdTurn, 2);
                    }
                    else
                    {
                        
                        var currentplayer = Players.FirstOrDefault(x => x.Id == IdTurn);
                        if (currentplayer.Cards.Any(x => x.Name == "Wild +4" || x.Name == "Wild +2"))
                        {
                            if (p.Cards.Count == 0)
                            {
                                _= NotifyPlayerPlaceCard(idPlayer, -1, new CardDTO()
                                {
                                    Color = c.Color,
                                    Id = c.Id,
                                    Name = c.Name
                                });
                                 _= GameFinished(idPlayer);

                                return;
                            }
                            else
                            {
                                _= NotifyPlayerPlaceCard(idPlayer, IdTurn, new CardDTO()
                                {
                                    Color = c.Color,
                                    Id = c.Id,
                                    Name = c.Name
                                });
                            }

                            AcumulatedCards += 2;
                        }
                        else
                        {
                            _= NotifyPlayerPlaceCard(idPlayer, -1, new CardDTO()
                            {
                                Color = c.Color,
                                Id = c.Id,
                                Name = c.Name
                            });
                            if (p.Cards.Count == 0)
                            {
                                 _= GameFinished(idPlayer);

                                return;
                            }

                            _ = TakeXCards(IdTurn, AcumulatedCards);
                        }


                    }




                }
                else
                {
                    if (c.Name == "Skip")
                    {
                        SkipTurn();
                    }

                    if (c.Name == "Reverse")
                    {
                        ReverseTurn();
                    }

                    if (NextTurn())
                    {
                        if (p.Cards.Count == 0)
                        {
                            _= NotifyPlayerPlaceCard(idPlayer, -1, new CardDTO()
                            {
                                Color = c.Color,
                                Id = c.Id,
                                Name = c.Name
                            });
                             _= GameFinished(idPlayer);

                            return;
                        }
                        else
                        {
                            _= NotifyPlayerPlaceCard(idPlayer, IdTurn, new CardDTO()
                            {
                                Color = c.Color,
                                Id = c.Id,
                                Name = c.Name
                            });
                        }


                    }
                    else
                    {
                        _= NotifyPlayerPlaceCard(idPlayer, -1, new CardDTO()
                        {
                            Color = c.Color,
                            Id = c.Id,
                            Name = c.Name
                        });
                        if (p.Cards.Count == 0)
                        {


                             _= GameFinished(idPlayer);

                            return;
                        }

                        _ = SearchNextPlayer();
                    }
                }


            }
            else
            {
                throw new Exception("Movimiento invalido");
            }





           

        }

        public async Task TakeXCards(int playerid, int cards)
        {
            AcumulatedCards = 0;
            for (int i = 0; i < cards - 1; i++)
            {
                await NotifyPlayerTakeCard(playerid, -1, TakeCard(playerid));
            }
            if (NextTurn())
            {
                await NotifyPlayerTakeCard(playerid, IdTurn, TakeCard(playerid));
            }
            else
            {
                await NotifyPlayerTakeCard(playerid, -1, TakeCard(playerid));
                _ = SearchNextPlayer();
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

            if (p == null)
            {
                throw new Exception("Jugador no encontrado");
            }

            if (!p.Cards.Any(c => c.Id == dto.IdCard))
            {
                throw new Exception("El jugador no tiene esa carta");
            }
            var c = Cards.First(c => c.Id == dto.IdCard);

            if (c.Name == "Wild +4" && !NewRules && p.Cards.Any(x=>x.Name==TopCard.Name || x.Color==LastColor))
            {
                throw new Exception("La carrta +4 solo puede ser utilizada como la ultima opcion en las reglas estandard");
            
            }


                if (c.Color != "black")
            {
                throw new Exception("Esta carta no permite cambio de color");

            }
            else
            {
                TopCard = c;
                LastColor = dto.Color;
                p.Cards.Remove(c);
                UsedCards.Add(c.Id);
                if (p.Id == IdTurn)
                {
                    Timer.Stop();
                }

                //verificar que si son las reglas originales no
                //la pueda agregar mas que como ultimo recurso
                //pero que si son las nuevas reglas no pase nada
                if (c.Name == "Wild +4")
                {
                    //cambiar las reglas aqui
                    SkipTurn();
                    if (!NewRules)
                    {


                        

                        _= NotifyPlayerPlaceCard(idPlayer, -1, new CardDTO()
                        {
                            Color = dto.Color,
                            Id = c.Id,
                            Name = c.Name
                        });
                        if (p.Cards.Count == 0)
                        {
                            _= GameFinished(idPlayer);

                            return;
                        }

                        _ = TakeXCards(IdTurn, 4);
                    }
                    else
                    {

                        var currentplayer = Players.FirstOrDefault(x => x.Id == IdTurn);
                        if (currentplayer.Cards.Any(x => x.Name == "Wild +4" || (x.Name == "+2" && x.Color==dto.Color)))
                        {
                            if (p.Cards.Count == 0)
                            {
                                _= NotifyPlayerPlaceCard(idPlayer, -1, new CardDTO()
                                {
                                    Color = dto.Color,
                                    Id = c.Id,
                                    Name = c.Name
                                });
                                 _= GameFinished(idPlayer);

                                return;
                            }
                            else
                            {
                                _= NotifyPlayerPlaceCard(idPlayer, IdTurn, new CardDTO()
                                {
                                    Color = dto.Color,
                                    Id = c.Id,
                                    Name = c.Name
                                });
                            }

                            AcumulatedCards += 4;
                        }
                        else
                        {
                            _= NotifyPlayerPlaceCard(idPlayer, -1, new CardDTO()
                            {
                                Color = dto.Color,
                                Id = c.Id,
                                Name = c.Name
                            });
                            if (p.Cards.Count == 0)
                            {

                                 _= GameFinished(idPlayer);

                                return;
                            }

                            _ = TakeXCards(IdTurn, AcumulatedCards);
                        }


                    }





                }
                else
                {
                    //es un wild solo de cambio de color
                    if (NextTurn())
                    {
                        if (p.Cards.Count == 0)
                        {
                            _= NotifyPlayerPlaceCard(idPlayer, -1, new CardDTO()
                            {
                                Color = dto.Color,
                                Id = c.Id,
                                Name = c.Name
                            });
                           
                             _= GameFinished(idPlayer);

                            return;
                        }
                        else
                        {
                            _= NotifyPlayerPlaceCard(idPlayer, IdTurn, new CardDTO()
                            {
                                Color = dto.Color,
                                Id = c.Id,
                                Name = c.Name
                            });
                        }


                    }
                    else
                    {
                        _= NotifyPlayerPlaceCard(idPlayer, -1, new CardDTO()
                        {
                            Color = dto.Color,
                            Id = c.Id,
                            Name = c.Name
                        }); 
                        if (p.Cards.Count == 0)
                        {
                             _= GameFinished(idPlayer);

                            return;
                        }

                        _ = SearchNextPlayer();
                    }
                }



            }


        }


        public async Task NotifyPlayerPlaceCard(int idPlayer, int turn, CardDTO dTO)
        {
            foreach (var player in Players)
            {
                if (player.Id == idPlayer)
                {
                    //continue;
                }
                await Notifications.PlayerColocoCard(player.Id.ToString(), new MovementDTO
                {
                    IdPlayer = idPlayer,
                    Card = dTO,
                    IdTurn = turn,
                    Reverse = IsReversed
                });
            }
            if (turn != -1)
            {
                PlayerstartTime = DateTime.Now;
                Timer.Start();
            }
            else
            {
                await Task.Delay(1000);

            }

        }

        public async Task GameFinished(int idPlayer)
        {
            await Task.Delay(1000);

            List<int> playersIds = Players.OrderBy(x=>x.Cards.Count()).Select(x=>x.Id).ToList();

            foreach (var player in Players)
            {
                if (player.Id == idPlayer)
                {
                    //continue;
                }
                await Notifications.GameFinished(player.Id.ToString(), new GanadorDTO()
                {
                    players= playersIds
                });
            }
            ReplaystarTime = DateTime.Now;
            ReplayTimer.Start();
        }



        public async Task NotifyPlayerTakeCard(int idPlayer, int turn, int cardid)
        {
            await Task.Delay(1000);

            foreach (var player in Players)
            {
                if (player.Id == idPlayer)
                {
                    await Notifications.YouTakeCard(player.Id.ToString(), new CardTaked()
                    {
                        IdTurn = turn,
                        Card = new CardDTO
                        {
                            Id = cardid,
                            Color = Cards.First(ca => ca.Id == cardid).Color,
                            Name = Cards.First(ca => ca.Id == cardid).Name
                        }
                    });
                    continue;
                }
                await Notifications.PlayerTakeCard(player.Id.ToString(), new TakedCard()
                {
                    IdPlayer = idPlayer,
                    IdTurn = turn
                });
            }
            if (turn != -1)
            {
                PlayerstartTime = DateTime.Now;
                Timer.Start();
            }
            else { 
                await Task.Delay(1000);

            }




        }











        public async Task SearchNextPlayer()
        {
            while (true)
            {
                var currentPlayer = Players.FirstOrDefault(pl => pl.Id == IdTurn);
                if (currentPlayer != null)
                {
                    int tcard = TakeCard(currentPlayer.Id);
                    if (!currentPlayer.Cards.Any(x => x.Color == "black" || x.Color == LastColor || x.Name == TopCard.Name))
                    {
                        //no tiene una carta valida aun asi que no puede jugar
                        if (!NewRules)
                        {
                            if (NextTurn())
                            {
                                //si el siguiente turno si tiene una carta valida asi que se le pasa el turno 
                                await NotifyPlayerTakeCard(currentPlayer.Id, IdTurn, tcard);
                                break;
                            }
                            else
                            {
                                //el siguiente no tiene carta valida asi que el turno queda vacio y se vuelve a empezar el bucle
                                await NotifyPlayerTakeCard(currentPlayer.Id, -1, tcard);
                            }
                        }
                        else
                        {
                            //si se esta jugando con las nuevas reglas entonces tiene que comer hasta que le toque una que pueda usar
                            await NotifyPlayerTakeCard(currentPlayer.Id, -1, tcard);
                        }
                        //si no es vuelve a entrar al bucle aqui se va a agregar la logica para las reglas 
                        //secundarias
                    }
                    else
                    {
                        //obtuvo una carta valida puede jugar
                        await NotifyPlayerTakeCard(currentPlayer.Id, IdTurn, tcard);
                        break;
                    }
                }
            }
        }




        public bool NextTurn()
        {
            // Buscar el nodo del jugador actual
            if (Players.Count == 0)
                return false;

            // Buscar el nodo del jugador actual
            var node = Players.First;
            while (node != null && node.Value.Id != IdTurn)
                node = node.Next;

            // 🔴 SI EL JUGADOR YA NO EXISTE
            if (node == null)
            {
                node = Players.First; // ← REANCLAR EL NODO
                IdTurn = node.Value.Id;
            }

            LinkedListNode<IPlayer> nextNode;

            if (!IsReversed)
            {
                nextNode = node.Next ?? Players.First;
            }
            else
            {
                nextNode = node.Previous ?? Players.Last;
            }

            IdTurn = nextNode.Value.Id;







            var currentPlayer = Players.FirstOrDefault(pl => pl.Id == IdTurn);
            if (currentPlayer == null)
            {
                throw new Exception("Jugador no encontrado");
            }
            if (currentPlayer.Cards.Any(x => x.Color == "black" || x.Color == LastColor || x.Name == TopCard.Name))
            {
                return true;
            }
            return false;
            //notificar
        }

        public void ResetGame()
        {
        }

        public void ReverseTurn()
        {
            IsReversed = !IsReversed;
        }

        public void SkipTurn()
        {
            var node = Players.First;
            while (node != null && node.Value.Id != IdTurn)
                node = node.Next;

            if (node == null)
            {
                // Si por alguna razón el turno se perdió, reseteamos al primero
                IdTurn = Players.First.Value.Id;
                return;
            }

            LinkedListNode<IPlayer> nextNode;

            if (!IsReversed)
            {
                // Modo normal: avanzar hacia adelante
                nextNode = node.Next ?? Players.First;
            }
            else
            {
                // Modo reversed: avanzar hacia atrás
                nextNode = node.Previous ?? Players.Last;
            }

            IdTurn = nextNode.Value.Id;
        }

        public void StartGame()
        {
            //ordenar al
            //eatoriamente los jugadores
            AutoStartTimer.Stop();

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
            var firstcard = NotUsed.FirstOrDefault(x => Cards.FirstOrDefault(c => c.Id == x).Color != "black" && Players.FirstOrDefault().Cards.Any(y=> y.Color==Cards.FirstOrDefault(c => c.Id == x).Color));
            UsedCards.Add(firstcard);
            NotUsed.Remove(firstcard);

            TopCard = Cards.FirstOrDefault(x => x.Id == UsedCards.First());
            LastColor = TopCard.Color;



            Timer.Elapsed += playerOut;
            Timer.AutoReset = false;
            Timer.Interval = 30000; // 60 segundos
            PlayerstartTime = DateTime.Now;

            Timer.Start();


            foreach (var player in Players)
            {
                Notifications.GameStarted(player.Id.ToString());
            }
        }

        public void StopGame()
        {
            throw new NotImplementedException();
        }

        public async Task NotifyPlayerLeft(int idPlayer, int turn)
        {
            await Task.Delay(1000);

            foreach (var player in Players)
            {

                await Notifications.PlayerLeft(player.Id.ToString(), new PlayerLeftDTO()
                {
                    IdPlayer = idPlayer,
                    IdTurn = turn
                });
                   
            }
           


        }


        public async void playerOut(object? sender, ElapsedEventArgs e)
        {
            /// eliminar jugador por inactividad

            ///notificar a los demas jugadores
            ///si es su turno, pasar al siguiente
            ///
            int idPlayer = IdTurn;

            var node = Players.First;
            while (node != null && node.Value.Id != idPlayer)
                node = node.Next;

            if (node == null)
                return;

            // Guardar siguiente ANTES de eliminar
            var nextNode = !IsReversed
                ? node.Previous ?? Players.Last
                : node.Next ?? Players.First;

            Players.Remove(node);

            if (Players.Count == 0)
                return;

            IdTurn = nextNode.Value.Id;

            if (NextTurn())
            {


                await NotifyPlayerLeft(idPlayer, IdTurn);
                PlayerstartTime = DateTime.Now;
                Timer.Start();


            }
            else
            {
                await NotifyPlayerLeft(idPlayer, -1);
                _ = SearchNextPlayer();
            }
            if(Players.Count == 1)
            {
                //terminar juego
                await GameFinished(Players.First.Value.Id);
                Close();
            }

        }

    }
}

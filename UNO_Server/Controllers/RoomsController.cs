using Serilog;
using UNO.Contract;
using UNO_Server.Hubs;
using UNO_Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UNO_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly MyHub _myHub;
        private readonly RoomContext _context;
        private readonly StartModel _startModel;
        private readonly DTOConverter _dtoConverter;

        public RoomsController(RoomContext context, MyHub myHub)
        {
            _myHub = myHub;
            _context = context;
            _startModel = new StartModel(context);
            _dtoConverter = new DTOConverter();
        }

        // GET: api/API
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            Log.Information("GET triggered.");
            if (_context.RoomItems == null)
            {
                return NotFound();
            }

            var allRooms = await _context.RoomItems.Include(item => item.Players).ThenInclude(item => item.PlayerHand)
                .Include(item => item.Cards).Include(item => item.MiddleCard).ToListAsync();

            return allRooms;
        }

        // GET: api/API/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(long id)
        {
            Log.Information("GET ID triggered.");
            if (_context.RoomItems == null)
            {
                return NotFound();
            }

            var roomItem = await _context.RoomItems.FindAsync(id);

            if (roomItem == null)
            {
                return NotFound();
            }

            return roomItem;
        }

        // PUT: api/API/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(long id, RoomDTO roomItem)
        {
            Log.Information("Put triggered.");
            if (id != roomItem.Id)
            {
                return BadRequest();
            }

            var room = _context.RoomItems.Include(r => r.Cards).Include(room => room.Players)
                .First(r => r.Id.Equals(roomItem.Id));

            foreach (var player in room.Players)
            {
                var existingPlayer = await _context.Players.FindAsync(player.Id);
                if (existingPlayer != null)
                {
                    // Update existing player
                    _context.Entry(existingPlayer).CurrentValues.SetValues(roomItem);
                    _context.Entry(existingPlayer).State = EntityState.Modified;
                }
                else
                {
                    // Add new player
                    _context.Players.Add(player);
                }
            }

            _context.RoomItems.Update(room);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            await _myHub.SendGetAllRooms("putSended");
            return NoContent();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private readonly Random _random = new Random();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPut("startroom/{roomid}")]
        public async Task<IActionResult> RoomId(long roomid, RoomDTO roomItem)
        {
            Log.Information($"Room {roomid} started.");

            var room = _context.RoomItems.Include(r => r.Cards).Include(room => room.MiddleCard)
                .Include(room => room.Center).Include(room => room.Players).First(r => r.Id.Equals(roomItem.Id));

            room.Cards.Clear();

            foreach (var card in _startModel.cards)
            {
                room.Cards.Add(card);
            }

            Log.Information("Die Center Karte wurde ermittelt und gelegt.");

            var randomCard = _random.Next(room.Cards.Count);
            var selectedCard = room.Cards[randomCard];

            if (selectedCard.Color == "Wild" || selectedCard.Color == "Draw")
            {
                while (selectedCard.Color == "Wild" || selectedCard.Color == "Draw")
                {
                    randomCard = _random.Next(room.Cards.Count);
                    selectedCard = room.Cards[randomCard];
                }
            }

            room.Cards.RemoveAt(randomCard);
            room.Center.Add(selectedCard);
            room.MiddleCard = room.Center.First();
            room.SelectedCard = room.MiddleCard;
            room.MiddleCardPic = room.MiddleCard.ImageUri;



            List<int> playerIds = new List<int>();
            foreach (var player in room.Players)
            {
                playerIds.Add((int)player.Id);
            }

            int minId = playerIds.Min();
            int maxId = playerIds.Max();
            
            room.StartingPlayer = _random.Next(minId, maxId);
            room.PlayerTurnId = room.StartingPlayer;
            if (room.PlayerTurnId != room.Players.Count)
            {
                room.NextPlayer = room.PlayerTurnId + 1;
            }
            else
            {
                room.NextPlayer = 1;
            }

            await _startModel.ShuffleDeck(room);
            await _startModel.DealCards(room);
            room.MoveCounter = 1;

            _context.RoomItems.Update(room);
            await _context.SaveChangesAsync();

            await _myHub.ConnectToRoom("roomStarted");
            await _myHub.SendGetAllRooms("roomStartedSended");

            return NoContent();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPut("placecard/{card}")]
        public async Task<IActionResult> PlaceCard(string card, RoomDTO roomItem)
        {
            Log.Information($"{card} gelegt.");

            var room = _context.RoomItems.Include(r => r.Cards).Include(room => room.Center)
                .Include(room => room.Players).ThenInclude(player => player.PlayerHand).Include(room => room.MiddleCard)
                .First(r => r.Id.Equals(roomItem.Id));
            var players = _context.Players.Include(player => player.PlayerHand)
                .First(p => p.RoomId.Equals(roomItem.Id));

            string[] splitted = card.Split("-");
            string color = splitted[0];
            string value = splitted[1];
            int cardId = Convert.ToInt32(splitted[2]);
            if (splitted.Length >= 4)
            {
                foreach (var player in room.Players)
                {
                    foreach (var playerCard in player.PlayerHand)
                    {
                        if ((int)playerCard.Id == cardId)
                        {
                            string wildOrDraw = splitted[3];
                            int number = Convert.ToInt32(splitted[4]);
                            if (wildOrDraw == "Draw")
                            {
                                room.Center.Add(_startModel.Draw4Cards[number]);
                                room.MiddleCard = room.Center.Last();
                                room.SelectedCard = room.MiddleCard;
                                room.MiddleCardPic = room.MiddleCard.ImageUri;
                                player.PlayerHand.Remove(playerCard);
                                break;
                            }
                            else if (wildOrDraw == "Wild")
                            {
                                room.Center.Add(_startModel.WildCards[number]);
                                room.MiddleCard = room.Center.Last();
                                room.SelectedCard = room.MiddleCard;
                                room.MiddleCardPic = room.MiddleCard.ImageUri;
                                player.PlayerHand.Remove(playerCard);
                                break;
                            }
                        }
                    }
                }
            }

            string path = $"pack://application:,,,/Assets/cards/{value}/{color}.png";

            // Prüfen ob die Karte passt, Farbe oder Zahl gleich oder Draw/Wild.
            if (color == "Wild" || color == "Draw")
            {
                if (value == "+4")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        var rndCard = _random.Next(room.Cards.Count);
                        var selectedCard = room.Cards[rndCard];
                        room.Players[room.NextPlayer - 1].PlayerHand.Add(selectedCard);
                    }
                }
            }
            else if (room.MiddleCard.Color == color || room.MiddleCard.Value == value)
            {
                room.Center.Add(new Card() { Color = color, Value = value, ImageUri = path });
                room.MiddleCard = room.Center.Last();
                room.SelectedCard = room.MiddleCard;
                room.MiddleCardPic = room.MiddleCard.ImageUri;
                foreach (var player in room.Players)
                {
                    foreach (var playerCard in player.PlayerHand)
                    {
                        if ((int)playerCard.Id == cardId)
                        {
                            player.PlayerHand.Remove(playerCard);

                            if (value == "+2")
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    var rndCard = _random.Next(room.Cards.Count);
                                    var selectedCard = room.Cards[rndCard];
                                    room.Players[room.NextPlayer - 1].PlayerHand.Add(selectedCard);
                                }
                            }
                            else if (playerCard.Value == "Skip")
                            {
                                room.IsSkip = true;
                            }
                            else if (playerCard.Value == "Reverse")
                            {
                                if (room.Players.Count == 2)
                                {
                                    room.IsSkip = true;
                                }
                                else
                                {
                                    if (room.IsReverse)
                                    {
                                        room.IsReverse = false;
                                    }
                                    else
                                    {
                                        room.IsReverse = true;
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
            }

            _context.RoomItems.Update(room);
            await _context.SaveChangesAsync();

            await _myHub.SendGetAllRooms("placeCard");

            return NoContent();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPut("playerendmove/{playerId}")]
        public async Task<IActionResult> PlayerEndMove(int playerId, RoomDTO roomItem)
        {
            Log.Information($"{playerId} hat seinen Zug beendet.");

            var room = _context.RoomItems.Include(r => r.Cards).Include(room => room.Center)
                .Include(room => room.Players).ThenInclude(player => player.PlayerHand).Include(room => room.MiddleCard)
                .First(r => r.Id.Equals(roomItem.Id));
            var players = _context.Players.Include(player => player.PlayerHand)
                .First(p => p.RoomId.Equals(roomItem.Id));

            if (room.Players[playerId - 1].PlayerHand.Count == 0)
            {
                await _myHub.OpenWinnerPage(room.Players[playerId - 1].Name + "-" + room.MoveCounter);
            }
            else
            {
                foreach (var player in room.Players)
                {
                    if (room.IsReverse && room.PlayerTurnId == playerId)
                    {
                        if (room.PlayerTurnId == 1)
                        {
                            room.PlayerTurnId = room.Players.Count;
                            if (room.IsSkip)
                            {
                                room.PlayerTurnId = room.Players.Count - 1;
                                room.IsSkip = false;
                            }

                            if (room.PlayerTurnId == 1)
                            {
                                room.NextPlayer = room.Players.Count;
                            }
                            else
                            {
                                room.NextPlayer = room.PlayerTurnId - 1;
                            }

                            // MoveCounter++;
                            // RoundCounterString = $"Runde: {MoveCounter}/\u221e";
                        }
                        else
                        {
                            room.PlayerTurnId--;
                            if (room.IsSkip)
                            {
                                if (room.PlayerTurnId == 1)
                                {
                                    room.PlayerTurnId = room.Players.Count;
                                }
                                else
                                {
                                    room.PlayerTurnId -= 1;
                                }

                                room.IsSkip = false;
                            }

                            if (room.PlayerTurnId == 1)
                            {
                                room.NextPlayer = room.Players.Count;
                            }
                            else
                            {
                                room.NextPlayer = room.Players.Count;
                            }
                        }

                        break;
                    }

                    if (!room.IsReverse && room.PlayerTurnId == playerId)
                    {
                        if (room.PlayerTurnId == room.Players.Count)
                        {
                            room.PlayerTurnId = 1;
                            if (room.IsSkip)
                            {
                                room.PlayerTurnId += 1;
                                room.IsSkip = false;
                            }

                            if (room.PlayerTurnId == room.Players.Count)
                            {
                                room.NextPlayer = 1;
                            }
                            else
                            {
                                room.NextPlayer = room.PlayerTurnId + 1;
                            }

                            // MoveCounter++;
                            // RoundCounterString = $"Runde: {MoveCounter}/\u221e";
                        }
                        else
                        {
                            room.PlayerTurnId++;
                            if (room.IsSkip)
                            {
                                if (room.PlayerTurnId == room.Players.Count)
                                {
                                    room.PlayerTurnId = 1;
                                }
                                else
                                {
                                    room.PlayerTurnId += 1;
                                }

                                room.IsSkip = false;
                            }

                            if (room.PlayerTurnId == room.Players.Count)
                            {
                                room.NextPlayer = 1;
                            }
                            else
                            {
                                room.NextPlayer = room.PlayerTurnId + 1;
                            }
                        }

                        break;
                    }
                }
            }

            room.MoveCounter++;

            _context.RoomItems.Update(room);
            await _context.SaveChangesAsync();

            await _myHub.SendGetAllRooms("playerToMove");

            return NoContent();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPut("unoclicked/{playerId}")]
        public async Task<IActionResult> UnoClicked(int playerId, RoomDTO roomItem)
        {
            Log.Information($"Player {playerId} clicked Uno.");

            var room = _context.RoomItems.Include(r => r.Cards).Include(room => room.MiddleCard)
                .Include(room => room.Center).Include(room => room.Players).ThenInclude(player => player.PlayerHand)
                .First(r => r.Id.Equals(roomItem.Id));

            if (room.Players[playerId].PlayerHand.Count == 1)
            {
                room.Players[playerId].Uno = true;
            }

            _context.RoomItems.Update(room);
            await _context.SaveChangesAsync();

            await _myHub.ConnectToRoom("roomStarted");
            await _myHub.SendGetAllRooms("roomStartedSended");

            return NoContent();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPut("drawCard/{playerName}")]
        public async Task<IActionResult> DrawCard(string playerName, RoomDTO roomItem)
        {
            Log.Information("DrawCard triggered.");

            var player = _context.Players.First(p => p.Name.Equals(playerName));
            var room = _context.RoomItems.Include(r => r.Cards).First(r => r.Id.Equals(roomItem.Id));

            var card = room.Cards.First();
            player.PlayerHand.Add(card);
            room.Cards.Remove(card);

            _context.Players.Update(player);
            _context.RoomItems.Update(room);
            await _context.SaveChangesAsync();

            await _myHub.SendGetAllRooms("drawCard");

            return NoContent();
        }
        
        
        [HttpPut("resetroom/{playerName}")]
        public async Task<IActionResult> ResetRoom(string playerName, RoomDTO roomItem)
        {
            Log.Information("DrawCard triggered.");

            var player = _context.Players.First(p => p.Name.Equals(playerName));
            var room = _context.RoomItems.Include(r => r.Cards).First(r => r.Id.Equals(roomItem.Id));

            room.Cards.Clear();
            room.Center.Clear();
            room.MiddleCard = new Card() { Color = "", Value = "", ImageUri = "" };
            room.MiddleCardPic = "";
            room.SelectedCard = new Card() { Color = "", Value = "", ImageUri = "" };

            _context.Players.Update(player);
            _context.RoomItems.Update(room);
            await _context.SaveChangesAsync();

            await _myHub.SendGetAllRooms("drawCard");

            return NoContent();
        }

        [HttpPut("updatemaximalplayers/{selectedMaximalUsers}")]
        public async Task<IActionResult> UpdateMaximalPlayers(int selectedMaximalUsers, RoomDTO roomItem)
        {
            Log.Information("UpdateMaximalPlayers triggered.");

            var room = _context.RoomItems.Include(r => r.Cards).First(r => r.Id.Equals(roomItem.Id));

            room.MaximalUsers = selectedMaximalUsers;

            _context.RoomItems.Update(room);
            await _context.SaveChangesAsync();

            await _myHub.SendGetAllRooms("UpdateMaximalPlayersSended");

            return NoContent();
        }

        [HttpPut("addPlayer/{playerName}")]
        public async Task<IActionResult> AddPlayerToRoom(string playerName, RoomDTO roomItem)
        {
            Log.Information("Player added.");
            var player = _context.Players.FirstOrDefault(p => p.Name.Equals(playerName));
            var room = _context.RoomItems.Include(r => r.Cards).Include(room => room.Players)
                .First(r => r.Id.Equals(roomItem.Id));

            if (player == null)
            {
                bool isLeader = room.Players.Count == 0;

                player = (await _context.Players.AddAsync(new Player()
                    { Name = playerName, RoomId = room.Id, IsLeader = isLeader })).Entity;
            }

            if (room.OnlineUsers != room.MaximalUsers)
            {
                room.Players.Add(player);
                room.OnlineUsers++;
            }

            _context.RoomItems.Update(room);
            await _context.SaveChangesAsync();

            await _myHub.SendGetAllRooms("addPlayerSended");

            return NoContent();
        }

        [HttpPut("removePlayer/{id}")]
        public async Task<IActionResult> RemovePlayerFromRoom(string id, RoomDTO roomItem)
        {
            Log.Information("Player removed.");
            var room = _context.RoomItems.Include(r => r.Cards).First(r => r.Id.Equals(roomItem.Id));

            room.OnlineUsers--;

            _context.RoomItems.Update(room);
            await _myHub.SendGetAllRooms("removePlayerSended");
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/API
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoomDTO>> PostRoom(RoomDTO roomDto)
        {
            Log.Information("Post triggered.");

            Room room = _dtoConverter.DtoConverterMethod(roomDto);

            _context.RoomItems.Add(room);
            await _context.SaveChangesAsync();
            // var room2 = _context.RoomItems.Include(r => r.Cards).First(r => r.Id.Equals(roomDto.Id));

            await _myHub.SendGetAllRooms("postSended");

            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }

        // DELETE: api/API/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(long id)
        {
            Log.Information("Delete triggered.");
            var roomItem = await _context.RoomItems.FindAsync(id);
            if (roomItem == null)
            {
                return NotFound();
            }

            _context.RoomItems.Remove(roomItem);
            await _context.SaveChangesAsync();

            await _myHub.SendGetAllRooms("removePlayerSended");

            return NoContent();
        }

        private bool RoomExists(long id)
        {
            return (_context.RoomItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
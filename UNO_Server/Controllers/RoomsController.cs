using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using UNO_Server.Models;
using UNO_Server.Hubs;
using UNO.Contract;
using Serilog;

namespace UNO_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly MyHub _myHub;
        private readonly RoomContext _context;
        private readonly StartModel _startModel;
        private readonly EndMoveModel _endMoveModel;
        private readonly PlaceCardModel _placeCardModel;
        private readonly DtoConverter _dtoConverter;

        public RoomsController(RoomContext context, MyHub myHub)
        {
            _myHub = myHub;
            _context = context;
            _startModel = new StartModel(context);
            _endMoveModel = new EndMoveModel();
            _placeCardModel = new PlaceCardModel();
            _dtoConverter = new DtoConverter();
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
        public async Task<IActionResult> PutRoom(long id, RoomDto roomItem)
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
            }

            await _myHub.SendGetAllRooms("putSended");
            return NoContent();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private readonly Random _random = new Random();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPut("startroom/{roomid}")]
        public async Task<IActionResult> RoomId(long roomid, RoomDto roomItem)
        {
            Log.Information($"Room {roomid} started.");
            var room = _context.RoomItems.Include(r => r.Cards).Include(room => room.MiddleCard)
                .Include(room => room.Center).Include(room => room.Players).First(r => r.Id.Equals(roomItem.Id));

            room.Cards.Clear();
            foreach (var card in _startModel.Cards)
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
        public async Task<IActionResult> PlaceCard(string card, RoomDto roomItem)
        {
            Log.Information($"{card} gelegt.");
            var room = _context.RoomItems
                .Include(r => r.Cards)
                .Include(room => room.Center)
                .Include(room => room.Players).ThenInclude(player => player.PlayerHand)
                .Include(room => room.MiddleCard)
                .First(r => r.Id.Equals(roomItem.Id));

            string[] splitted = card.Split("-");
            string color = splitted[0];
            string value = splitted[1];
            int cardId = Convert.ToInt32(splitted[2]);

            if (splitted.Length >= 4)
            {
                _placeCardModel.HandleSpecialCards(room, splitted, cardId, _startModel);
            }
            if (color is "Wild" or "Draw")
            {
                _placeCardModel.HandleWildDrawCards(room, value);
            }
            else if (room.MiddleCard.Color == color || room.MiddleCard.Value == value)
            {
                _placeCardModel.HandleStandardCard(room, color, value, cardId);
            }

            _context.RoomItems.Update(room);
            await _context.SaveChangesAsync();
            await _myHub.SendGetAllRooms("placeCard");
            return NoContent();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPut("playerendmove/{playerId}")]
        public async Task<IActionResult> PlayerEndMove(int playerId, RoomDto roomItem)
        {
            var room = _context.RoomItems.Include(r => r.Cards).Include(room => room.Center)
                .Include(room => room.Players).ThenInclude(player => player.PlayerHand).Include(room => room.MiddleCard)
                .First(r => r.Id.Equals(roomItem.Id));

            Log.Information($"{playerId} hat seinen Zug beendet.");

            List<int> ids = _endMoveModel.CreateIdList(room);
            int minId = ids.Min();
            int maxId = ids.Max();

            var player = _context.Players.Include(player => player.PlayerHand).First(p => p.Id.Equals(playerId));

            if (player.Uno && player.PlayerHand.Count == 0)
            {
                await _myHub.OpenWinnerPage(room.Players[room.PlayerTurnId].Name + "-" + room.MoveCounter);
            }
            else if (!player.Uno && player.PlayerHand.Count == 0)
            {
                room.Players[room.PlayerTurnId].PlayerHand.Add(room.Cards.First());
                room.Cards.Remove(room.Cards.First());
            }
            else
            {
                switch (room.IsReverse)
                {
                    case true:
                        _endMoveModel.IsReverse(room, minId, maxId);
                        break;
                    case false:
                        _endMoveModel.IsNotReverse(room, minId, maxId);
                        break;
                }
            }

            _context.RoomItems.Update(room);
            await _context.SaveChangesAsync();
            await _myHub.SendGetAllRooms("playerEndMove");

            return NoContent();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPut("unoclicked/{playerId}")]
        public async Task<IActionResult> UnoClicked(int playerId, RoomDto roomItem)
        {
            var room = _context.RoomItems.Include(r => r.Cards).Include(room => room.MiddleCard)
                .Include(room => room.Center).Include(room => room.Players).ThenInclude(player => player.PlayerHand)
                .First(r => r.Id.Equals(roomItem.Id));
            Log.Information($"Player {playerId} clicked Uno.");

            if (room.Players[playerId - 1].PlayerHand.Count <= 1)
            {
                room.Players[playerId - 1].Uno = true;
            }

            _context.RoomItems.Update(room);
            await _context.SaveChangesAsync();
            await _myHub.ConnectToRoom("roomStarted");
            await _myHub.SendGetAllRooms("roomStartedSended");

            return NoContent();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPut("drawCard/{playerName}")]
        public async Task<IActionResult> DrawCard(string playerName, RoomDto roomItem)
        {
            var player = _context.Players.First(p => p.Name.Equals(playerName));
            var room = _context.RoomItems.Include(r => r.Cards).First(r => r.Id.Equals(roomItem.Id));
            Log.Information("DrawCard triggered.");

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
        public async Task<IActionResult> ResetRoom(string playerName, RoomDto roomItem)
        {
            var room = _context.RoomItems.Include(r => r.Cards).First(r => r.Id.Equals(roomItem.Id));
            Log.Information("DrawCard triggered.");

            room.Cards.Clear();
            room.Center.Clear();
            var emptyCard = new Card() { Color = "", Value = "", ImageUri = "" };
            room.MiddleCardPic = "";
            room.MiddleCard = emptyCard;
            room.SelectedCard = emptyCard;

            _context.RoomItems.Update(room);
            await _context.SaveChangesAsync();
            await _myHub.SendGetAllRooms("drawCard");

            return NoContent();
        }

        [HttpPut("updatemaximalplayers/{selectedMaximalUsers}")]
        public async Task<IActionResult> UpdateMaximalPlayers(int selectedMaximalUsers, RoomDto roomItem)
        {
            var room = _context.RoomItems.Include(r => r.Cards).First(r => r.Id.Equals(roomItem.Id));
            Log.Information("UpdateMaximalPlayers triggered.");

            room.MaximalUsers = selectedMaximalUsers;

            _context.RoomItems.Update(room);
            await _context.SaveChangesAsync();
            await _myHub.SendGetAllRooms("UpdateMaximalPlayersSended");

            return NoContent();
        }

        [HttpPut("addPlayer/{playerName}")]
        public async Task<IActionResult> AddPlayerToRoom(string playerName, RoomDto roomItem)
        {
            var player = _context.Players.FirstOrDefault(p => p.Name.Equals(playerName));
            var room = _context.RoomItems.Include(r => r.Cards).Include(room => room.Players)
                .First(r => r.Id.Equals(roomItem.Id));
            Log.Information("Player added.");

            if (player == null)
            {
                bool isLeader = room.Players.Count == 0;

                player = (await _context.Players.AddAsync(new Player()
                    { Name = playerName, RoomId = room.Id, IsLeader = isLeader })).Entity;
            }

            if (room.Players.Count >= 2)
            {
                room.StartButtonEnabled = true;
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
        public async Task<IActionResult> RemovePlayerFromRoom(string id, RoomDto roomItem)
        {
            Log.Information("Player removed.");
            var room = _context.RoomItems.Include(r => r.Cards).Include(room => room.Players)
                .First(r => r.Id.Equals(roomItem.Id));

            if (room.Players.Count <= 2)
            {
                room.StartButtonEnabled = false;
            }

            room.OnlineUsers--;

            _context.RoomItems.Update(room);
            await _myHub.SendGetAllRooms("removePlayerSended");
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/API
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoomDto>> PostRoom(RoomDto roomDto)
        {
            Log.Information("Post triggered.");
            Room room = _dtoConverter.DtoConverterMethod(roomDto);

            _context.RoomItems.Add(room);
            await _context.SaveChangesAsync();
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
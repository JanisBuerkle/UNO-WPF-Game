using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
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
        private readonly RoomContext _context;
        private readonly MyHub _myHub;
        private readonly StartModel _startModel;
        private readonly DTOConverter _dtoConverter;

        public RoomsController(RoomContext context, MyHub myHub)
        {
            _startModel = new StartModel(context);
            _myHub = myHub;
            _context = context;
            _dtoConverter = new DTOConverter();
        }

        // GET: api/API
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            Log.Information("GET triggered.");
            // await _hubContext.Clients.All.SendAsync("EmpfangeNachricht", "Test");
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

            var room = _context.RoomItems.Include(r => r.Cards).Include(room => room.Players).First(r => r.Id.Equals(roomItem.Id));
            foreach (var player in room.Players)
            {
                var existingPlayer = _context.Players.Find(player.Id);
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
            foreach (var card in _startModel.WildCards)
            {
                room.Cards.Add(card);
            }

            foreach (var card in _startModel.Draw4Cards)
            {
                room.Cards.Add(card);
            }

            foreach (var card in _startModel.cards)
            {
                room.Cards.Add(card);
            }

            Log.Information("Die Center Karte wurde ermittelt und gelegt.");
            var randomCard = _random.Next(room.Cards.Count);
            var selectedCard = room.Cards[randomCard];
            room.Cards.RemoveAt(randomCard);
            room.Center.Add(selectedCard);


            if (room.MiddleCard.Color == "Wild" || room.MiddleCard.Color == "Draw")
            {
            }

            room.MiddleCard = room.Center.First();
            room.SelectedCard = room.MiddleCard;
            var middleCardPath = room.MiddleCard.ImageUri;
            room.MiddleCardPic = middleCardPath;


            room.StartingPlayer = _random.Next(0, room.Players.Count);
            await _startModel.ShuffleDeck(room);
            await _startModel.DealCards(room);


            _context.RoomItems.Update(room);
            await _context.SaveChangesAsync();


            await _myHub.SendGetAllRooms("roomStartedSended");

            return NoContent();
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPut("placecard/{card}")]
        public async Task<IActionResult> PlaceCard(string card, RoomDTO roomItem)
        {
            Log.Information($"{card} gelegt.");

            var room = _context.RoomItems.Include(r => r.Cards).First(r => r.Id.Equals(roomItem.Id));
            string color = Regex.Match(card, "[A-Za-z]+").Value;
            string value = Regex.Match(card, "\\d+").Value;

            string path = $"pack://application:,,,/Assets/cards/{value}/{color}.png";
            roomItem.Center.Add(new CardDTO() { Color = color, Value = value, ImageUri = path });

            _context.RoomItems.Update(room);
            await _context.SaveChangesAsync();

            await _myHub.SendGetAllRooms("placeCard");

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
            var room = _context.RoomItems.Include(r => r.Cards).First(r => r.Id.Equals(roomItem.Id));
            
            if (player == null)
            {
                bool isLeader = room.Players.Count == 0;

                player = (await _context.Players.AddAsync(
                    new Player() { Name = playerName, RoomId = room.Id, IsLeader = isLeader })).Entity;
            }

            room.Players.Add(player);
            room.OnlineUsers++;

            _context.RoomItems.Update(room);
            await _context.SaveChangesAsync();

            await _myHub.SendGetAllRooms("addPlayerSended");

            return NoContent();
        }

        [HttpPut("removePlayer/{playerName}")]
        public async Task<IActionResult> RemovePlayerFromRoom(string playerName, RoomDTO roomItem)
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
            var room2 = _context.RoomItems.Include(r => r.Cards).First(r => r.Id.Equals(roomDto.Id));

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
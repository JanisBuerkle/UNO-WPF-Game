using Serilog;
using UNO.Contract;
using UNO_Server.Hubs;
using UNO_Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace UNO_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly RoomContext _context;
        private readonly IHubContext<MyHub> _hubContext;
        private readonly MyHub _myHub;
        private readonly StartModel _startModel;

        public RoomsController(RoomContext context, IHubContext<MyHub> hubContext, MyHub myHub)
        {
            _startModel = new StartModel(context);
            _myHub = myHub;
            _context = context;
            _hubContext = hubContext;
        }

        // GET: api/API
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDTO>>> GetTodoItems()
        {
            Log.Information("GET triggered.");
            // await _hubContext.Clients.All.SendAsync("EmpfangeNachricht", "Test");
            if (_context.RoomItems == null)
            {
                return NotFound();
            }

            var allRooms = await _context.RoomItems.Include(item => item.Players).ThenInclude(item => item.PlayerHand)
                .Include(item => item.Cards).ToListAsync();

            return allRooms;
        }

        // GET: api/API/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDTO>> GetTodoItem(long id)
        {
            Log.Information("GET ID triggered.");
            if (_context.RoomItems == null)
            {
                return NotFound();
            }

            var todoItem = await _context.RoomItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/API/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, RoomDTO roomItem)
        {
            Log.Information("Put triggered.");
            if (id != roomItem.Id)
            {
                return BadRequest();
            }

            foreach (var player in roomItem.Players)
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

            _context.RoomItems.Update(roomItem);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
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

            roomItem.Cards.Clear();
            foreach (var card in _startModel.WildCards)
            {
                roomItem.Cards.Add(card);
            }

            foreach (var card in _startModel.Draw4Cards)
            {
                roomItem.Cards.Add(card);
            }

            foreach (var card in _startModel.cards)
            {
                roomItem.Cards.Add(card);
            }
            
            Log.Information("Die Center Karte wurde ermittelt und gelegt.");
            var randomCard = _random.Next(roomItem.Cards.Count);
            var selectedCard = roomItem.Cards[randomCard];
            roomItem.Cards.RemoveAt(randomCard);
            roomItem.Center.Add(selectedCard);

            int startingPlayer = _random.Next(0, roomItem.Players.Count);
            await _startModel.ShuffleDeck(roomItem);
            await _startModel.DealCards(roomItem);


            _context.RoomItems.Update(roomItem);
            await _context.SaveChangesAsync();


            await _myHub.SendGetAllRooms("roomStartedSended");

            return NoContent();
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPut("drawCard/{playerName}")]
        public async Task<IActionResult> UpdateMaximalPlayers(string playerName, RoomDTO roomItem)
        {
            Log.Information("DrawCard triggered.");

            foreach (var player in roomItem.Players)
            {
                if (player.Name == playerName)
                {
                    player.PlayerHand.Add(roomItem.Cards.First());
                    roomItem.Cards.Remove(roomItem.Cards.First());
                }
            }

            _context.RoomItems.Update(roomItem);
            await _context.SaveChangesAsync();

            await _myHub.SendGetAllRooms("drawCard");

            return NoContent();
        }
        
        [HttpPut("updatemaximalplayers/{selectedMaximalUsers}")]
        public async Task<IActionResult> UpdateMaximalPlayers(int selectedMaximalUsers, RoomDTO roomItem)
        {
            Log.Information("UpdateMaximalPlayers triggered.");

            roomItem.MaximalUsers = selectedMaximalUsers;

            _context.RoomItems.Update(roomItem);
            await _context.SaveChangesAsync();

            await _myHub.SendGetAllRooms("UpdateMaximalPlayersSended");

            return NoContent();
        }

        [HttpPut("addPlayer/{playerName}")]
        public async Task<IActionResult> AddPlayerToRoom(string playerName, RoomDTO roomItem)
        {
            Log.Information("Player added.");
            var player = _context.Players.FirstOrDefault(p => p.Name.Equals(playerName));
            if (player == null)
            {
                bool isLeader = roomItem.Players.Count == 0;

                player = (await _context.Players.AddAsync(new MultiplayerDTO()
                    { Name = playerName, RoomId = roomItem.Id, IsLeader = isLeader })).Entity;
            }

            roomItem.Players.Add(player);
            roomItem.OnlineUsers++;

            _context.RoomItems.Update(roomItem);
            await _context.SaveChangesAsync();

            await _myHub.SendGetAllRooms("addPlayerSended");

            return NoContent();
        }

        [HttpPut("removePlayer/{playerName}")]
        public async Task<IActionResult> RemovePlayerFromRoom(string playerName, RoomDTO roomItem)
        {
            Log.Information("Player removed.");

            roomItem.OnlineUsers--;

            _context.RoomItems.Update(roomItem);
            await _myHub.SendGetAllRooms("removePlayerSended");
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/API
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoomDTO>> PostTodoItem(RoomDTO roomItem)
        {
            Log.Information("Post triggered.");
            _context.RoomItems.Add(roomItem);
            await _context.SaveChangesAsync();

            await _myHub.SendGetAllRooms("postSended");

            //    return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return CreatedAtAction(nameof(GetTodoItem), new { id = roomItem.Id }, roomItem);
        }

        // DELETE: api/API/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            Log.Information("Delete triggered.");
            var todoItem = await _context.RoomItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.RoomItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            await _myHub.SendGetAllRooms("removePlayerSended");

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return (_context.RoomItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
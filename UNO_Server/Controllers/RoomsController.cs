using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UNO_Server.Models;
using UNO_Server.ViewModel;

namespace UNO_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly RoomContext _context;

        public RoomsController(RoomContext context)
        {
            _context = context;
        }

        // GET: api/API
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomItem>>> GetTodoItems()
        {
            if (_context.RoomItems == null)
            {
                return NotFound();
            }

            return await _context.RoomItems.Include(item => item.PlayerNames).ToListAsync();
        }

        // GET: api/API/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomItem>> GetTodoItem(long id)
        {
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
        public async Task<IActionResult> PutTodoItem(long id, RoomItem roomItem)
        {
            if (id != roomItem.Id)
            {
                return BadRequest();
            }

            foreach (var player in roomItem.PlayerNames)
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

            return NoContent();
        }

        [HttpPut("addPlayer/{playerName}")]
        public async Task<IActionResult> AddPlayerToRoom(string playerName, RoomItem roomItem)
        {
            var player = _context.Players.FirstOrDefault(p => p.Name.Equals(playerName));
            if (player == null)
            {
                player = (await _context.Players.AddAsync(new MultiplayerPlayer { Name = playerName })).Entity;
            }

            roomItem.PlayerNames.Add(player);
            roomItem.OnlineUsers++;

            _context.RoomItems.Update(roomItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("removePlayer/{playerName}")]
        public async Task<IActionResult> RemovePlayerFromRoom(string playerName, RoomItem roomItem)
        {
            var player = _context.Players.FirstOrDefault(p => p.Name.Equals(playerName));
            if (player == null)
            {
                return NotFound();
            }

            roomItem.PlayerNames.Remove(player);
            roomItem.OnlineUsers--;

            _context.RoomItems.Update(roomItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/API
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoomItem>> PostTodoItem(RoomItem roomItem)
        {
            _context.RoomItems.Add(roomItem);
            await _context.SaveChangesAsync();

            //    return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return CreatedAtAction(nameof(GetTodoItem), new { id = roomItem.Id }, roomItem);
        }

        // DELETE: api/API/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.RoomItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.RoomItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return (_context.RoomItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
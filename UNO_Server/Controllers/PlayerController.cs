using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UNO_Server.Hubs;
using UNO_Server.Models;
using UNO.Contract;

namespace UNO_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly MyHub _myHub;
        private readonly RoomContext _context;

        public PlayerController(RoomContext context, MyHub myHub)
        {
            _myHub = myHub;
            _context = context;
        }

        // GET: api/Player
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            if (_context.Players == null)
            {
                return NotFound();
            }

            return await _context.Players.ToListAsync();
        }

        // GET: api/Player/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetMultiplayerPlayer(long id)
        {
            if (_context.Players == null)
            {
                return NotFound();
            }

            var multiplayerPlayer = await _context.Players.FindAsync(id);

            if (multiplayerPlayer == null)
            {
                return NotFound();
            }

            return multiplayerPlayer;
        }

        // PUT: api/Player/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMultiplayerPlayer(long id, PlayerDTO playerPlayer)
        {
            if (id != playerPlayer.Id)
            {
                return BadRequest();
            }

            _context.Entry(playerPlayer).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MultiplayerPlayerExists(id))
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

        // POST: api/Player
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PlayerDTO>> PostMultiplayerPlayer(PlayerDTO playerPlayer)
        {
            var player = _context.Players.First(p => p.Id.Equals(playerPlayer.Id));
            if (_context.Players == null)
            {
                return Problem("Entity set 'RoomContext.Players'  is null.");
            }

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMultiplayerPlayer", new { id = player.Id }, player);
        }

        // DELETE: api/Player/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMultiplayerPlayer(long id)
        {
            if (_context.Players == null)
            {
                return NotFound();
            }

            var multiplayerPlayer = await _context.Players.FindAsync(id);
            if (multiplayerPlayer == null)
            {
                return NotFound();
            }

            _context.Players.Remove(multiplayerPlayer);
            await _context.SaveChangesAsync();

            await _myHub.SendGetAllRooms("deletePlayerSended");
            return NoContent();
        }

        private bool MultiplayerPlayerExists(long id)
        {
            return (_context.Players?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
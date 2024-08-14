using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UNO_Server.Hubs;
using UNO_Server.Models;
using UNO.Contract;

namespace UNO_Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly MyHub myHub;
    private readonly RoomContext context;
    private readonly StartModel startModel;
    private readonly EndMoveModel endMoveModel;
    private readonly PlaceCardModel placeCardModel;
    private readonly DtoConverter dtoConverter;

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private readonly Random random = new();

    public RoomsController(RoomContext context, MyHub myHub)
    {
        this.myHub = myHub;
        this.context = context;
        startModel = new StartModel();
        endMoveModel = new EndMoveModel();
        placeCardModel = new PlaceCardModel();
        dtoConverter = new DtoConverter();
    }

    // GET: api/API
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
    {
        Log.Information("GET triggered.");
        if (context.RoomItems == null)
        {
            return NotFound();
        }

        var allRooms = await context.RoomItems.Include(item => item.Players).ThenInclude(item => item.PlayerHand)
            .Include(item => item.Cards).Include(item => item.MiddleCard).ToListAsync();

        return allRooms;
    }

    // GET: api/API/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Room>> GetRoom(long id)
    {
        Log.Information("GET ID triggered.");
        if (context.RoomItems == null)
        {
            return NotFound();
        }

        var roomItem = await context.RoomItems.FindAsync(id);

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

        var room = context.RoomItems.Include(r => r.Cards).Include(room => room.Players)
            .First(r => r.Id.Equals(roomItem.Id));

        foreach (var player in room.Players)
        {
            var existingPlayer = await context.Players.FindAsync(player.Id);
            if (existingPlayer != null)
            {
                // Update existing player
                context.Entry(existingPlayer).CurrentValues.SetValues(roomItem);
                context.Entry(existingPlayer).State = EntityState.Modified;
            }
            else
            {
                // Add new player
                context.Players.Add(player);
            }
        }

        context.RoomItems.Update(room);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RoomExists(id))
            {
                return NotFound();
            }
        }

        await myHub.SendGetAllRooms("putSended");
        return NoContent();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [HttpPut("startroom/{roomid}")]
    public async Task<IActionResult> RoomId(long roomid, RoomDto roomItem)
    {
        Log.Information($"Room {roomid} started.");
        var room = context.RoomItems.Include(r => r.Cards).Include(room => room.MiddleCard)
            .Include(room => room.Center).Include(room => room.Players).First(r => r.Id.Equals(roomItem.Id));

        room.Cards.Clear();
        foreach (var card in startModel.Cards)
        {
            room.Cards.Add(card);
        }

        Log.Information("Die Center Karte wurde ermittelt und gelegt.");

        var randomCard = random.Next(room.Cards.Count);
        var selectedCard = room.Cards[randomCard];

        if (selectedCard.Color == "Wild" || selectedCard.Color == "Draw")
        {
            while (selectedCard.Color == "Wild" || selectedCard.Color == "Draw")
            {
                randomCard = random.Next(room.Cards.Count);
                selectedCard = room.Cards[randomCard];
            }
        }

        room.Cards.RemoveAt(randomCard);
        room.Center.Add(selectedCard);
        room.MiddleCard = room.Center.First();
        room.SelectedCard = room.MiddleCard;
        room.MiddleCardPic = room.MiddleCard.ImageUri;

        var playerIds = new List<int>();
        foreach (var player in room.Players)
        {
            playerIds.Add((int)player.Id);
        }

        var minId = playerIds.Min();
        var maxId = playerIds.Max();

        room.StartingPlayer = random.Next(minId, maxId);
        room.PlayerTurnId = room.StartingPlayer;
        if (room.PlayerTurnId != room.Players.Count)
        {
            room.NextPlayer = room.PlayerTurnId + 1;
        }
        else
        {
            room.NextPlayer = 1;
        }

        await startModel.ShuffleDeck(room);
        await startModel.DealCards(room);
        room.MoveCounter = 1;

        context.RoomItems.Update(room);
        await context.SaveChangesAsync();
        await myHub.ConnectToRoom("roomStarted");
        await myHub.SendGetAllRooms("roomStartedSended");

        return NoContent();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [HttpPut("placecard/{card}")]
    public async Task<IActionResult> PlaceCard(string card, RoomDto roomItem)
    {
        Log.Information($"{card} gelegt.");
        var room = context.RoomItems.Include(r => r.Cards).Include(room => room.Center).Include(room => room.Players).ThenInclude(player => player.PlayerHand).Include(room => room.MiddleCard)
            .First(r => r.Id.Equals(roomItem.Id));
        var nextPlayer = context.Players.Include(player => player.PlayerHand).First(p => p.Id.Equals(room.NextPlayer));

        var splitted = card.Split("-");
        var color = splitted[0];
        var value = splitted[1];
        var cardId = Convert.ToInt32(splitted[2]);

        if (splitted.Length >= 4)
        {
            placeCardModel.HandleSpecialCards(room, splitted, cardId, startModel);
        }

        if (color is "Wild" or "Draw")
        {
            placeCardModel.HandleWildDrawCards(room, value, nextPlayer);
        }
        else if (room.MiddleCard.Color == color || room.MiddleCard.Value == value)
        {
            placeCardModel.HandleStandardCard(room, color, value, cardId, nextPlayer);
        }

        context.RoomItems.Update(room);
        context.Players.Update(nextPlayer);
        await context.SaveChangesAsync();
        await myHub.SendGetAllRooms("placeCard");
        return NoContent();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [HttpPut("playerendmove/{playerId}")]
    public async Task<IActionResult> PlayerEndMove(int playerId, RoomDto roomItem)
    {
        var player = context.Players.Include(player => player.PlayerHand).First(p => p.Id.Equals(playerId));
        var room = context.RoomItems.Include(r => r.Cards).Include(room => room.Center).Include(room => room.Players).ThenInclude(p => p.PlayerHand).Include(room => room.MiddleCard)
            .First(r => r.Id.Equals(roomItem.Id));
        Log.Information($"{playerId} hat seinen Zug beendet.");

        var ids = endMoveModel.CreateIdList(room);
        var minId = ids.Min();
        var maxId = ids.Max();

        if (player.Uno && player.PlayerHand.Count == 0)
        {
            await myHub.OpenWinnerPage(player.Name + "-" + room.MoveCounter);
        }
        else if (!player.Uno && player.PlayerHand.Count == 0)
        {
            player.PlayerHand.Add(room.Cards.First());
            room.Cards.Remove(room.Cards.First());
        }
        else
        {
            switch (room.IsReverse)
            {
                case true:
                    endMoveModel.IsReverse(room, minId, maxId);
                    break;
                case false:
                    endMoveModel.IsNotReverse(room, minId, maxId);
                    break;
            }
        }

        context.RoomItems.Update(room);
        context.Players.Update(player);
        await context.SaveChangesAsync();
        await myHub.SendGetAllRooms("playerEndMove");

        return NoContent();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [HttpPut("unoclicked/{playerId}")]
    public async Task<IActionResult> UnoClicked(int playerId, RoomDto roomItem)
    {
        var player = context.Players.Include(player => player.PlayerHand).First(p => p.Id.Equals(playerId));
        var room = context.RoomItems.Include(r => r.Cards).Include(room => room.MiddleCard).Include(room => room.Center).Include(room => room.Players).ThenInclude(p => p.PlayerHand)
            .First(r => r.Id.Equals(roomItem.Id));
        Log.Information($"Player {playerId} clicked Uno.");

        if (player.PlayerHand.Count <= 1)
        {
            player.Uno = true;
        }

        context.RoomItems.Update(room);
        context.Players.Update(player);
        await context.SaveChangesAsync();
        await myHub.ConnectToRoom("roomStarted");
        await myHub.SendGetAllRooms("roomStartedSended");

        return NoContent();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [HttpPut("drawCard/{playerName}")]
    public async Task<IActionResult> DrawCard(string playerName, RoomDto roomItem)
    {
        var player = context.Players.First(p => p.Name.Equals(playerName));
        var room = context.RoomItems.Include(r => r.Cards).First(r => r.Id.Equals(roomItem.Id));
        Log.Information("DrawCard triggered.");

        var card = room.Cards.First();
        player.PlayerHand.Add(card);
        room.Cards.Remove(card);

        context.Players.Update(player);
        context.RoomItems.Update(room);
        await context.SaveChangesAsync();
        await myHub.SendGetAllRooms("drawCard");

        return NoContent();
    }


    [HttpPut("resetroom/{playerName}")]
    public async Task<IActionResult> ResetRoom(string playerName, RoomDto roomItem)
    {
        var room = context.RoomItems.Include(r => r.Cards).First(r => r.Id.Equals(roomItem.Id));
        Log.Information("DrawCard triggered.");

        room.Cards.Clear();
        room.Center.Clear();
        var emptyCard = new Card() { Color = String.Empty, Value = String.Empty, ImageUri = String.Empty };
        room.MiddleCardPic = String.Empty;
        room.MiddleCard = emptyCard;
        room.SelectedCard = emptyCard;

        context.RoomItems.Update(room);
        await context.SaveChangesAsync();
        await myHub.SendGetAllRooms("drawCard");

        return NoContent();
    }

    [HttpPut("updatemaximalplayers/{selectedMaximalUsers}")]
    public async Task<IActionResult> UpdateMaximalPlayers(int selectedMaximalUsers, RoomDto roomItem)
    {
        var room = context.RoomItems.Include(r => r.Cards).First(r => r.Id.Equals(roomItem.Id));
        Log.Information("UpdateMaximalPlayers triggered.");

        room.MaximalUsers = selectedMaximalUsers;

        context.RoomItems.Update(room);
        await context.SaveChangesAsync();
        await myHub.SendGetAllRooms("UpdateMaximalPlayersSended");

        return NoContent();
    }

    [HttpPut("addPlayer/{playerInformations}")]
    public async Task<IActionResult> AddPlayerToRoom(string playerInformations, RoomDto roomItem)
    {
        var p = playerInformations.Split("-");
        var playerName = p[0];
        var connectionId = p[1];
        var player = context.Players.FirstOrDefault(p => p.Name.Equals(playerName));
        var room = context.RoomItems.Include(r => r.Cards).Include(room => room.Players).First(r => r.Id.Equals(roomItem.Id));
        Log.Information("Player added.");

        if (player == null)
        {
            var isLeader = room.Players.Count == 0;

            player = (await context.Players.AddAsync(new Player { Name = playerName, ConnectionId = connectionId, RoomId = room.Id, IsLeader = isLeader })).Entity;
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

        context.RoomItems.Update(room);
        await context.SaveChangesAsync();
        await myHub.SendGetAllRooms("addPlayerSended");

        return NoContent();
    }

    [HttpPut("removePlayer/{id}")]
    public async Task<IActionResult> RemovePlayerFromRoom(string id, RoomDto roomItem)
    {
        Log.Information("Player removed.");
        foreach (var roomFromList in context.RoomItems.Include(r => r.Players))
        {
            foreach (var player in roomFromList.Players)
            {
                if (player.ConnectionId == id)
                {
                    var room = context.RoomItems.Include(r => r.Cards).Include(r => r.Players).First(r => r.Id.Equals(roomFromList.Id));
                    if (room.Players.Count <= 2)
                    {
                        room.StartButtonEnabled = false;
                    }

                    room.OnlineUsers--;
                    
                    var httpClient = new HttpClient();
                    var removePlayerUrl = $"http://localhost:5000/api/Player/{player.Id}";
                    var response = await httpClient.DeleteAsync(removePlayerUrl);

                    context.RoomItems.Update(room);
                    await myHub.SendGetAllRooms("removePlayerSended");
                    await context.SaveChangesAsync();
                }
            }
        }

        return NoContent();
    }

    // POST: api/API
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<RoomDto>> PostRoom(RoomDto roomDto)
    {
        Log.Information("Post triggered.");
        var room = dtoConverter.DtoConverterMethod(roomDto);

        context.RoomItems.Add(room);

        await context.SaveChangesAsync();
        await myHub.SendGetAllRooms("postSended");
        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
    }

    // DELETE: api/API/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(long id)
    {
        Log.Information("Delete triggered.");
        var roomItem = await context.RoomItems.FindAsync(id);
        if (roomItem == null)
        {
            return NotFound();
        }

        context.RoomItems.Remove(roomItem);
        await context.SaveChangesAsync();
        await myHub.SendGetAllRooms("removePlayerSended");
        return NoContent();
    }

    private bool RoomExists(long id)
    {
        return (context.RoomItems?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
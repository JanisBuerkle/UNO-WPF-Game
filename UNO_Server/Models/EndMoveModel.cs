namespace UNO_Server.Models;

public class EndMoveModel
{
    public List<int> CreateIdList(Room room)
    {
        return room.Players.Select(player => (int)player.Id).ToList();
    }

    public void IsReverse(Room room, int minId, int maxId)
    {
        if (room.PlayerTurnId == minId)
        {
            room.MoveCounter++;
            room.PlayerTurnId = maxId;
            if (room.IsSkip)
            {
                room.PlayerTurnId = maxId - 1;
                room.IsSkip = false;
            }

            if (room.PlayerTurnId == minId)
            {
                room.NextPlayer = maxId;
            }
            else
            {
                room.NextPlayer = room.PlayerTurnId - 1;
            }
        }
        else
        {
            room.PlayerTurnId--;
            if (room.IsSkip)
            {
                if (room.PlayerTurnId == minId)
                {
                    room.PlayerTurnId = maxId;
                }
                else
                {
                    room.PlayerTurnId -= 1;
                }

                room.IsSkip = false;
            }

            if (room.PlayerTurnId == minId)
            {
                room.NextPlayer = maxId;
            }
            else
            {
                room.NextPlayer = room.PlayerTurnId - 1;
            }
        }
    }

    public void IsNotReverse(Room room, int minId, int maxId)
    {
        if (room.PlayerTurnId == maxId)
        {
            room.MoveCounter++;
            room.PlayerTurnId = minId;
            if (room.IsSkip)
            {
                room.PlayerTurnId += 1;
                room.IsSkip = false;
            }

            if (room.PlayerTurnId == maxId)
            {
                room.NextPlayer = minId;
            }
            else
            {
                room.NextPlayer = room.PlayerTurnId + 1;
            }
        }
        else
        {
            room.PlayerTurnId++;
            if (room.IsSkip)
            {
                if (room.PlayerTurnId == maxId)
                {
                    room.PlayerTurnId = minId;
                }
                else
                {
                    room.PlayerTurnId += 1;
                }

                room.IsSkip = false;
            }

            if (room.PlayerTurnId == maxId)
            {
                room.NextPlayer = minId;
            }
            else
            {
                room.NextPlayer = room.PlayerTurnId + 1;
            }
        }
    }
}
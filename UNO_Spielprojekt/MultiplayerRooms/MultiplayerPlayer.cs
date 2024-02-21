namespace UNO_Spielprojekt.MultiplayerRooms;

public class MultiplayerPlayer
{
    public long Id { get; set; }
    public string Name { get; set; }

    private bool Equals(MultiplayerPlayer other)
    {
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((MultiplayerPlayer)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
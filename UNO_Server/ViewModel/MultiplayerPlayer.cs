﻿namespace UNO_Server.ViewModel;

public class MultiplayerPlayer
{
    public long Id { get; set; }
    public long RoomId { get; set; }
    public string Name { get; set; }
    public bool IsLeader { get; set; }
}
﻿using UNO_Server.ViewModel;

namespace UNO_Server.Models;

public class RoomItem : ViewModelBase
{
    public long Id { get; set; }
    public string RoomName { get; set; }
    
    private int _onlineUsers;
    public int OnlineUsers
    {
        get => _onlineUsers;
        set
        {
            if (_onlineUsers != value)
            {
                _onlineUsers = value;
                OnPropertyChanged();
            }
        }
    }
    public int MaximalUsers { get; set;}
    public bool PasswordSecured { get; set;}
    public string Password { get; set;}
    public List<MultiplayerPlayer> Players { get; set; } = new List<MultiplayerPlayer>();
}
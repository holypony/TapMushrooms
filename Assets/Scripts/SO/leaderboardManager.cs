using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LeaderBoardManager", menuName = "Data/LeaderBoardManager")]

public class leaderboardManager : ScriptableObject
{
    public List<string> LeadersList;

    [SerializeField] private bool _value;
    [SerializeField] private int totalPlayer;
    [SerializeField] private int totalGamePlayed;
    [SerializeField] private string username;

    public bool Value
    {
        get => _value;
        set
        {
            _value = value;
            OnValueChange?.Invoke(_value);
        }

    }
    
    public int TotalPlayers
    {
        get => totalPlayer;
        set => totalPlayer = value;
    }
    
    public string Username
    {
        get => username;
        set => username = value;
    }
    
    public int TotalGamesPlayed
    {
        get => totalGamePlayed;
        set
        {
            totalGamePlayed = value;

        }

    }
    
    public event Action<bool> OnValueChange;
}
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Data/GameSo")]
public class GameManagerSo : ScriptableObject
{

    [Header("Game settings")]
    [SerializeField] private int score;
    [SerializeField] private int bestScore;
    [Space(15)]
    [SerializeField] private int hp;
    [SerializeField] private bool gameOver;
    [SerializeField] private bool isPause;
    [Space(15)]
    [SerializeField] private int bombMushroomsLive;
    [SerializeField] private float mushLifeTime;
    [Space(15)]
    [SerializeField] private int multiplier;
    [SerializeField] private float timeBetweenSpawn;
    [SerializeField] private float defaultTimeBetweenSpawn;
    [SerializeField] private float diffIndex;

    private void Awake()
    {
        InitializeGameSo();
    }

    public void InitializeGameSo()
    {
        Multiplier = 1;
        MushLifeTime = 1f;
        BombMushroomsLive = 0;
        defaultTimeBetweenSpawn = 0.35f;
        TimeBetweenSpawn = defaultTimeBetweenSpawn;
        DiffIndex = 0;

        Score = 0;
        Hp = 3;
        GameOver = false;
    }

    private float multiplierTimeBonus;

    public void UpdateTimeBetweenSpawns()
    {
        DiffIndex = -Score / 7500f;

        if (Multiplier > 1)
        {
            multiplierTimeBonus = -(defaultTimeBetweenSpawn + DiffIndex) / 2;
        }
        else
        {
            multiplierTimeBonus = 0;
        }

        TimeBetweenSpawn = defaultTimeBetweenSpawn + DiffIndex + multiplierTimeBonus;
    }
    
    public float MushLifeTime
    {
        get => mushLifeTime;
        set => mushLifeTime = value;
    }

    public float TimeBetweenSpawn
    {
        get => timeBetweenSpawn;
        set
        {
            if (value < 0.1f)
            {
                value = 0.1f;
            }

            timeBetweenSpawn = value;
        }
    }
    public int Score
    {
        get => score;
        set
        {
            score = value;
            OnScoreChange?.Invoke();
        }
    }
    
    public float DiffIndex
    {
        get => diffIndex;
        set
        {
            diffIndex = value;
        }
    }
    
    public int BestScore
    {
        get => bestScore;
        set
        {
            bestScore = value;
            OnBestScoreChange?.Invoke();
        }
    }
    
    public int Multiplier
    {
        get => multiplier;
        set
        {
            multiplier = value;
            OnMultiplierChange?.Invoke(value);
        }
    }

    public int BombMushroomsLive
    {
        get => bombMushroomsLive;
        set { bombMushroomsLive = value; }
    }
    
    public int Hp
    {
        get => hp;
        set
        {
            hp = value;
            if (hp < 1)
            {
                hp = 0;
                GameOver = true;
            }
            OnHpChange?.Invoke();
        }
    }

    public bool GameOver
    {
        get => gameOver;
        set
        {
            gameOver = value;
            OnGameOverChange?.Invoke(value);
        }
    }
    
    public bool IsPause
    {
        get => isPause;
        set
        {
            isPause = value;
            OnPauseChange?.Invoke(value);
        }
    }


    public event Action<bool> OnGameOverChange;
    public event Action<bool> OnPauseChange;
    public event Action OnScoreChange;
    public event Action OnBestScoreChange;
    public event Action<int> OnMultiplierChange;
    public event Action OnHpChange;

}
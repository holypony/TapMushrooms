using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/GameSo")]
public class GameManagerSo : ScriptableObject
{

    [Header("Game settings")]
    [SerializeField] private int score;
    [SerializeField] private int bestScore;
    [SerializeField] private int hp;
    [SerializeField] private bool gameOver;

    [SerializeField] private float mushLifeTime;
    [SerializeField] private float timeBetweenSpawn;
    [SerializeField] private float defaultTimeBetweenSpawn;
    [SerializeField] private int multiplier = 1;
    [SerializeField] private int bombSpawnChance = 13;
 

    private void Awake()
    {
        InitializeGameSo();
    }

    public void InitializeGameSo()
    {

        MushLifeTime = 1.5f;
        BombSpawnChance = 13;
        defaultTimeBetweenSpawn = 0.75f;
        Multiplier = 1;
        TimeBetweenSpawn = defaultTimeBetweenSpawn;
        Score = 0;
        Hp = 3;
        GameOver = false;
        
    }
    
    
    public int Multiplier
    {
        get => multiplier;
        set
        {
            if (value < 1)
                value = 1;
            multiplier = value;
            TimeBetweenSpawn = defaultTimeBetweenSpawn / multiplier;
            OnMultiplierChange?.Invoke();
        }
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
            if (value < 0.15f)
            {
                value = 0.15f;
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
    
    public int BestScore
    {
        get => bestScore;
        set
        {
            bestScore = value;
            OnBestScoreChange?.Invoke();
        }
    }

    public void AddScore(int addScore)
    {
        Score += addScore * (int)Multiplier;
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
            OnGameOverChange?.Invoke();
        }

    }
    
    public int BombSpawnChance
    {
        get => bombSpawnChance;
        set
        {
            bombSpawnChance = value;
        }

    }

    


    public event Action OnGameOverChange;
    public event Action OnMultiplierChange;
    public event Action OnScoreChange;
    public event Action OnBestScoreChange;
    public event Action OnHpChange;

}
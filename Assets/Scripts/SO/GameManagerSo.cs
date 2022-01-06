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
    [SerializeField] private int hp;
    [SerializeField] private bool gameOver;

    [SerializeField] private int multiplier;
    [SerializeField] private float mushLifeTime;
    [SerializeField] private float timeBetweenSpawn;
    [SerializeField] private float defaultTimeBetweenSpawn;
    
    
   
    
    
    private void Awake()
    {
        InitializeGameSo();
    }
    
   
    
    public void InitializeGameSo()
    {
        Multiplier = 1;
        MushLifeTime = 1.5f;
        
        defaultTimeBetweenSpawn = 0.65f;
        TimeBetweenSpawn = defaultTimeBetweenSpawn;
        
        Score = 0;
        Hp = 3;
        GameOver = false;
        
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
    
    public int Multiplier
    {
        get => multiplier;
        set
        {
            multiplier = value;
            OnMultiplierChange?.Invoke();
        }
    }

    public void AddScore(int addScore)
    {
        Score += addScore * Multiplier;
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


    public event Action OnGameOverChange;
    public event Action OnScoreChange;
    public event Action OnBestScoreChange;
    public event Action OnMultiplierChange;
    public event Action OnHpChange;

}
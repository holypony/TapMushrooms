using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameManagerSo gameSo;
    [SerializeField] private leaderboardManager _leaderboardManager;
    public Mushroom[] mushrooms;
    private List<Mushroom> readyMushrooms;
    private bool isBonusTime = false;

    //DEMO
    [SerializeField] private GameObject DemoMush;
    [SerializeField] private GameObject Hand;
    private Animator DemoMushAnim;
    private Animator HandAnim;
    
    public static GameManager instance;
    private void Awake()
    {
        DemoMushAnim = DemoMush.GetComponent<Animator>();
        HandAnim = Hand.GetComponent<Animator>();
        
        Time.timeScale = 1;
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(this.gameObject); 
    }
    
    public void StartGame()
    {
        
        StopAllCoroutines();
        InitializeGameField();
        gameSo.InitializeGameSo();
        StartCoroutine(GameRoutine());
        
        PlayerPrefs.SetInt("totalGamePlayed", PlayerPrefs.GetInt("totalGamePlayed", 0) + 1);
        FirebaseAnalytics.instance.UpdateTotalGames();

        StartCoroutine(Demo());
    }

    private IEnumerator Demo()
    {
        
        yield return new WaitForSeconds(2f);
        DemoMushAnim.SetTrigger("Start");
        yield return new WaitForSeconds(2f);
        HandAnim.SetTrigger("Hand");
    }
    
    private void InitializeGameField()
    {
        isBonusTime = false;
        readyMushrooms = new List<Mushroom>();
        foreach (var mushroom in mushrooms)
        {
            mushroom.mushroomState(false);
        }
    }

    private IEnumerator GameRoutine()
    {
        while (!gameSo.GameOver)
        {
            foreach (var mushroom in mushrooms)
            {
                if (!mushroom.CheckMushroomState())
                {
                    readyMushrooms.Add(mushroom);
                }
            }

            if (readyMushrooms.Count > 0)
            {
                int i = Random.Range(0, readyMushrooms.Count);
                readyMushrooms[i].mushroomState(true);
                readyMushrooms.Clear();
            }
            
            if (!isBonusTime && gameSo.Score > 100)
            {

                if (setRandom(12))
                {
                    StartCoroutine(bonusTime());
                }
            }
            
            gameSo.UpdateTimeBetweenSpawns();
            yield return new WaitForSeconds(gameSo.TimeBetweenSpawn);
        }

        gameSo.BestScore = PlayerPrefs.GetInt("BestScore", 1);
        
        if (gameSo.BestScore < gameSo.Score)
        {
            gameSo.BestScore = gameSo.Score;
            PlayerPrefs.SetInt("BestScore", gameSo.Score);
        }
        
        FirebaseAnalytics.instance.AddBestScore();
        yield return new WaitForSeconds(0.15f);
        FirebaseAnalytics.instance.StartScoreboardLoader();
    }

    private IEnumerator bonusTime()
    {
        isBonusTime = true;
        int i = Random.Range(2, 5);
        gameSo.Multiplier = i;
        yield return new WaitForSeconds(5f);
        gameSo.Multiplier = 1;
        isBonusTime = false;
    }
    
    private bool setRandom(float setChance)
    {
        int drop = Random.Range(0, 101);
        return drop <= setChance;
    }
}

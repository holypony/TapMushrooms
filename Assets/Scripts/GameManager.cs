using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameManagerSo gameManagerSo;
    public Mushroom[] mushrooms;
    public List<Mushroom> readyMushrooms;
    private bool isBonusTime = false;

    [SerializeField] private GameGuide guide;
    public static GameManager instance;
    private void Awake()
    {
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
        if (PlayerPrefs.GetInt("guide", -1) < 0)
        {
            guide.InitGuide();
        }
        else
        {
            StopAllCoroutines();
            InitializeGameField();
            gameManagerSo.InitializeGameSo();
            StartCoroutine(GameRoutine());
        
            PlayerPrefs.SetInt("totalGamePlayed", PlayerPrefs.GetInt("totalGamePlayed", 0) + 1);
            FirebaseAnalytics.instance.UpdateTotalGames();
        }
    }

    public void InitializeGameField()
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
        while (!gameManagerSo.GameOver)
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
                if (gameManagerSo.BombMushroomsLive < 4 && getRandom(13))
                {
                    gameManagerSo.BombMushroomsLive++;
                    readyMushrooms[i].mushroomState(true, true);
                }
                else
                {
                    readyMushrooms[i].mushroomState(true);
                }
                
                readyMushrooms.Clear();
            }
            
            if (!isBonusTime && gameManagerSo.Score > 100)
            {

                if (getRandom(12))
                {
                    StartCoroutine(bonusTime());
                }
            }
            
            gameManagerSo.UpdateTimeBetweenSpawns();
            yield return new WaitForSeconds(gameManagerSo.TimeBetweenSpawn);
        }

        gameManagerSo.BestScore = PlayerPrefs.GetInt("BestScore", 1);
        
        if (gameManagerSo.BestScore < gameManagerSo.Score)
        {
            gameManagerSo.BestScore = gameManagerSo.Score;
            PlayerPrefs.SetInt("BestScore", gameManagerSo.Score);
        }
        yield return new WaitForSeconds(0.15f);
        FirebaseAnalytics.instance.StartScoreboardLoader();
    }

    private IEnumerator bonusTime()
    {
        isBonusTime = true;
        int i = Random.Range(2, 5);
        gameManagerSo.Multiplier = i;
        yield return new WaitForSeconds(5f);
        gameManagerSo.Multiplier = 1;
        isBonusTime = false;
    }
    
    private bool getRandom(float setChance)
    {
        int drop = Random.Range(0, 101);
        return drop <= setChance;
    }

}

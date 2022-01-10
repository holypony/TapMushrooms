using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameManagerSo gameSo;
    [SerializeField] private Mushroom[] mushrooms;
    private List<Mushroom> readyMushrooms;

    private bool isBonusTime = false;


    private void InitializeGameField()
    {
        isBonusTime = false;
        readyMushrooms = new List<Mushroom>();
        foreach (var mushroom in mushrooms)
        {
            mushroom.mushroomState(false);
        }
    }

    public void StartGame()
    {
        
        InitializeGameField();
        gameSo.InitializeGameSo();
        StopAllCoroutines();
        StartCoroutine(GameRoutine());
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

                if (setRandom(9))
                {
                    StartCoroutine(bonusTime());
                }
            }
            
            gameSo.UpdateTimeBetweenSpawns();
            yield return new WaitForSeconds(gameSo.TimeBetweenSpawn);
        }
    }
    
    

    private IEnumerator bonusTime()
    {
        isBonusTime = true;
        gameSo.Multiplier = 2;
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

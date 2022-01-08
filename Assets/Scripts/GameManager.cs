using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameManagerSo gameSo;
    [SerializeField] private Mushroom[] mushrooms;
    private List<Mushroom> ReadyMushrooms;

    private int lastMushIndex = 99;
    private bool _isBonusTime = false;

    private void Awake()
    {
        InitializeGameField();
    }

    private void InitializeGameField()
    {
        
        
        ReadyMushrooms = new List<Mushroom>();
        foreach (var mushroom in mushrooms)
        {
            mushroom.mushroomState(false);
            if (!mushroom.CheckMushroomState())
            {
                ReadyMushrooms.Add(mushroom);
            }
        }
    }



    public void StartGame()
    {
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
                    ReadyMushrooms.Add(mushroom);
                }
            }

            if (ReadyMushrooms.Count > 0)
            {
                int i = Random.Range(0, ReadyMushrooms.Count);
                ReadyMushrooms[i].mushroomState(true);
                ReadyMushrooms.Clear();
            }
            gameSo.UpdateTimeBetweenSpawns();
            
            if (!_isBonusTime && gameSo.Score > 100)
            {

                if (GetRandom(9))
                {
                    StartCoroutine(bonusTime());
                }
            }

            yield return new WaitForSeconds(gameSo.TimeBetweenSpawn);
        }

        InitializeGameField();
    }
    
    

    private IEnumerator bonusTime()
    {
        _isBonusTime = true;
        gameSo.Multiplier = 2;
        yield return new WaitForSeconds(5f);
        gameSo.Multiplier = 1;
        _isBonusTime = false;
    }


    private bool GetRandom(float setChance)
    {
        int drop = Random.Range(0, 101);
        return drop <= setChance;
    }
}

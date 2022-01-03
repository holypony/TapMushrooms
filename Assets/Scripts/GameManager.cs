using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameManagerSo gameSo;
    [SerializeField] private Mushroom[] mushrooms;

    public void InitializeGameField()
    {
        gameSo.InitializeGameSo();
        
        foreach (var t in mushrooms)
        {
            t.mushroomState(false);
        }
        StopAllCoroutines();
        StartCoroutine(Game());
    }

    private IEnumerator Game()
    {
        while(!gameSo.GameOver)
        {
            var mushIndex = Random.Range(0, mushrooms.Length);
            if (!mushrooms[mushIndex].IsMushroomDeactivated())
            {
                mushrooms[mushIndex].mushroomState(true);
            }

            if (GetRandom(20 / gameSo.Multiplier))
            {
                if (!_isMulti)
                {
                    StartCoroutine(addMulti());
                }
                else
                {
                    _multiTime += 3f;
                    gameSo.Multiplier++;

                }
            }
            yield return new WaitForSeconds(gameSo.TimeBetweenSpawn);
        }
    }

  
    private bool _isMulti = false;
    private float _multiTime = 0;
    IEnumerator addMulti()
    {
        _isMulti = true;


        _multiTime = 3f;
        gameSo.Multiplier++;
        float timer = 0f;
        while (_multiTime > 0)
        {
            timer += 0.1f;
            if ((Math.Round(timer) / 3) >= 1)
            {
                timer = 0;
                gameSo.Multiplier--;
            }
            _multiTime -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        _isMulti = false;
    }
    
    
    
    

    private bool GetRandom(float setChance)
    {
        int drop = Random.Range(0, 101);
        return drop <= setChance;
    }
}

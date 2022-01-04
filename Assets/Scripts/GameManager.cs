using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameManagerSo gameSo;
    [SerializeField] private Mushroom[] mushrooms;
    private int lastMushIndex = 99;
    private bool _isBonusTime = false;
    
    
    public void InitializeGameField()
    {
        gameSo.InitializeGameSo();
        
        foreach (var t in mushrooms)
        {
            t.mushroomState(false);
        }
        StopAllCoroutines();
        StartCoroutine(GameRoutine());
    }

    private IEnumerator GameRoutine()
    {
        while(!gameSo.GameOver)
        {
            while (!useFreeMushroom())
            {
                yield return null;
            }

            if (!_isBonusTime)
            {
                if (GetRandom(20))
                {
                    StartCoroutine(bonusTime(2));
                }
                else
                {
                    if (GetRandom(5)) StartCoroutine(bonusTime(3));
                }
            }
            
            yield return new WaitForSeconds(gameSo.TimeBetweenSpawn);
        }
    }

    
    bool useFreeMushroom()
    {
        var mushIndex = Random.Range(0, mushrooms.Length);
        
        if (mushIndex == lastMushIndex) return false;
        

        if (mushrooms[mushIndex].IsMushroomDeactivated()) return false;
        lastMushIndex = mushIndex;
        mushrooms[mushIndex].mushroomState(true);
        return true;

    }
    
    private IEnumerator bonusTime(int multiplier)
    {
        _isBonusTime = true;
        gameSo.Multiplier = multiplier;
        gameSo.TimeBetweenSpawn /= multiplier;
        yield return new WaitForSeconds(5f);
        gameSo.TimeBetweenSpawn  *= multiplier;
        gameSo.Multiplier = 1;
        _isBonusTime = false;
    }
    
  
    private bool GetRandom(float setChance)
    {
        int drop = Random.Range(0, 101);
        return drop <= setChance;
    }
}

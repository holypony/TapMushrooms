using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterAI : MonoBehaviour
{
    [SerializeField] private GameManager gm;

    public void StartTesterAi()
    {
        StartCoroutine(chkMushrooomsLife());
    }

    IEnumerator chkMushrooomsLife()
    {
        int i;
        while (true)
        {
            for(i = 0; i < gm.mushrooms.Length; i++)
            {
                if (gm.mushrooms[i].CheckMushroomState())
                {
                    StartCoroutine(ClickOnMushroom(i));
                }
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
    }

    IEnumerator ClickOnMushroom(int i)
    {
        yield return new WaitForSeconds(0.2f);
        gm.mushrooms[i].TesterClick();
    }
}

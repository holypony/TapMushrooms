using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterAI : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    private bool AiIsWork = false;
    public void StartTesterAi()
    {
        if (!AiIsWork)
        {
            StartCoroutine(chkMushrooomsLife());
        }
        else
        {
            AiIsWork = false;
        }
        
    }

    IEnumerator chkMushrooomsLife()
    {
        AiIsWork = true;
        int i;
        while (AiIsWork)
        {
            for(i = 0; i < gm.mushrooms.Length; i++)
            {
                if (gm.mushrooms[i].CheckMushroomState())
                {
                    StartCoroutine(ClickOnMushroom(i));
                    yield return new WaitForSeconds(0.25f);
                }
            }
            
            yield return new WaitForSeconds(0.01f);
        }
        
    }

    IEnumerator ClickOnMushroom(int i)
    {
        yield return new WaitForSeconds(0.1f);
        gm.mushrooms[i].TesterClick();
    }
}

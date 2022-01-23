using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterAI : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private bool AiIsWork = false;
    [SerializeField] private float timeBetweenClick = 0.15f;
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
            for (i = 0; i < gm.mushrooms.Length; i++)
            {
                if (gm.mushrooms[i].CheckMushroomState())
                {
                    //StartCoroutine(ClickOnMushroom(i));
                    yield return new WaitForSeconds(timeBetweenClick);
                    gm.mushrooms[i].TesterClick();
                }
            }

            yield return null;
        }
    }

    IEnumerator ClickOnMushroom(int i)
    {
        yield return new WaitForSeconds(0.1f);
        gm.mushrooms[i].TesterClick();
    }
}

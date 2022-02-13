using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGuide : MonoBehaviour
{
    [SerializeField] private GameObject UiFirstStep;
    [SerializeField] private GameObject UiSecondStep;
    [SerializeField] private GameObject UiThirdStep;
    
    [SerializeField] private GameManager gm;
    [SerializeField] private GameManagerSo gameManagerSo;
    
    public void InitGuide()
    {
        gm.InitializeGameField();
        gameManagerSo.InitializeGameSo(100f);
        
        foreach (var mushroom in gm.mushrooms)
        {
            if (!mushroom.CheckMushroomState())
            {
                gm.readyMushrooms.Add(mushroom);
            }
        }

        StartCoroutine(guideFirstAction());
    }

    private IEnumerator guideFirstAction()
    {
        UiFirstStep.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        gm.readyMushrooms[4].mushroomState(true);
    }

    public void GuideSecondPage()
    {
        gm.readyMushrooms[4].TesterClick();
        StartCoroutine(guideSecondAction());
    }
    
    private IEnumerator guideSecondAction()
    {
        UiFirstStep.SetActive(false);
        UiSecondStep.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        gm.readyMushrooms[4].mushroomState(true, true);
    }
    
    public void GuideTapOnRed()
    {
        gm.readyMushrooms[4].ShowDie();
    }

    public void GuideThirdPage()
    {
        if (gm.readyMushrooms[4].CheckMushroomState())
        {
            gm.readyMushrooms[4].mushroomState(false);
        }
        gameManagerSo.InitializeGameSo(1f);
        gm.readyMushrooms[4].mushroomState(true);
        UiSecondStep.SetActive(false);
        UiThirdStep.SetActive(true);
       
    }

    public void CloseGuide()
    {
        UiThirdStep.SetActive(false);
        PlayerPrefs.SetInt("guide", 0);
        gm.readyMushrooms[4].ShowDie();
        gm.readyMushrooms[4].mushroomState(false);
    }
    
    private IEnumerator guideThirdAction()
    {
        UiFirstStep.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        gm.readyMushrooms[4].mushroomState(true);
        
    }

    public void ResetGuide()
    {
        PlayerPrefs.SetInt("guide", -1);
        
    }
}

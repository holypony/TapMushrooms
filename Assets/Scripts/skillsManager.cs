using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class skillsManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameManagerSo gameSo;
    [SerializeField] private GameManager gm;
    
    [Header("Skills stettings")]
    [SerializeField] private float aiCooldown = 5f;
    [SerializeField] private float aiTimeBetweenTap = 0.1f;
    [SerializeField] private float aiDuration = 5f;
    
    [SerializeField] private float allKillCooldown = 5f;

    [Header("UI / animators")]
    [SerializeField] private TMP_Text textAiCooldown;
    [SerializeField] private TMP_Text textAllKillCooldown;
    [SerializeField] private Image imgAi;
    [SerializeField] private Image imgAllKill;
    
    private bool isKillReady = true;
    private bool isAiReady = true;

  
    public void StartAi(bool isGameOver)
    {
        if (isAiReady && !isGameOver)
        {
            StartCoroutine(aiRoutine());
        }
    }

    private IEnumerator aiRoutine()
    {
        float activeTime = aiDuration;
        isAiReady = false;
        imgAi.enabled = false;
        while (!gameSo.GameOver && activeTime > 0.1f)
        {
            for (int i = 0; i < gm.mushrooms.Length; i++)
            {
                if (gm.mushrooms[i].CheckMushroomState())
                {
                    yield return new WaitForSeconds(aiTimeBetweenTap);
                    activeTime -= aiTimeBetweenTap;
                    gm.mushrooms[i].TesterClick();
                }
                yield return null;
            }

            yield return null;
        }

        float currentCooldown = aiCooldown;
        while (currentCooldown > 0.1f )
        {
            textAiCooldown.text = currentCooldown.ToString();
            currentCooldown -= 1f;
            yield return new WaitForSeconds(1f);
        }

        textAiCooldown.text = "";
        imgAi.enabled = true;
        isAiReady = true;
    }
    
    public void KillAll()
    {
        if (isKillReady)
        {
            StartCoroutine(KillAllRoutine());
        }
    }
    
    private IEnumerator KillAllRoutine()
    {
        imgAllKill.enabled = false;
        isKillReady = false;
        for (int i = 8; i >= 0; i--)
        {
            gm.mushrooms[i].TesterClick(true);
            yield return new WaitForSeconds(0.05f);
        }
        
        float currentCooldown = allKillCooldown;
        while (currentCooldown > 0.1f )
        {
            textAllKillCooldown.text = currentCooldown.ToString();
            currentCooldown -= 1f;
            yield return new WaitForSeconds(1f);
        }

        textAllKillCooldown.text = "";
        isKillReady = true;
        imgAllKill.enabled = true;
    }
}

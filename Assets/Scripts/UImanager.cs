using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{

    [SerializeField] private GameObject panelStart;
    [SerializeField] private GameObject panelUIinGame;
    [SerializeField] private TMP_Text textInGameScore;
    [SerializeField] private TMP_Text textHp;
    [SerializeField] private TMP_Text textTimeBetweenSpawn;
    [SerializeField] private TMP_Text textMultiplier;

    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private TMP_Text textAfterGameScore;
    [SerializeField] private TMP_Text textBestScore;
    
    [SerializeField] private GameManagerSo gameSo;

  

    private void OnEnable()
    {
        gameSo.OnScoreChange += updateUI;
        gameSo.OnHpChange += updateUI;
        gameSo.OnGameOverChange += updateUI;
        gameSo.OnMultiplierChange += bonusTime;
        panelUIinGame.SetActive(true);
        panelStart.SetActive(true);
    }

    private void OnDisable()
    {
        gameSo.OnScoreChange -= updateUI;
        gameSo.OnHpChange -= updateUI;
        gameSo.OnGameOverChange -= updateUI;
        gameSo.OnMultiplierChange -= bonusTime;
    }

    private void bonusTime()
    {
        if (gameSo.Multiplier > 1)
        {
            textMultiplier.enabled = true;
            textMultiplier.text = "X" + gameSo.Multiplier;
        }
        else
        {
            textMultiplier.enabled = false;
        }
    }

    public void StartGame()
    {
        panelStart.SetActive(false);
    }

    private void updateUI()
    {
        textInGameScore.text = "Score: " + gameSo.Score;
        textHp.text = "HP: " + gameSo.Hp;
        textTimeBetweenSpawn.text = "Time between spawn: " + Math.Round(gameSo.TimeBetweenSpawn, 2) + " s";

        


        if (gameSo.GameOver)
        {
            if (PlayerPrefs.GetInt("BestScore", 0) < gameSo.Score)
            {
                gameSo.BestScore = gameSo.Score;
                PlayerPrefs.SetInt("BestScore", gameSo.Score);
            }
            textAfterGameScore.text = "Score: " + gameSo.Score;
            textBestScore.text = "Best score: " + PlayerPrefs.GetInt("BestScore", 0);
            panelUIinGame.SetActive(false);
            panelGameOver.SetActive(true);
        }
        else
        {
            panelUIinGame.SetActive(true);
            panelGameOver.SetActive(false);
        }
    }
}

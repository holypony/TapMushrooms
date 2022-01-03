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
        gameSo.OnScoreChange += UpdateUI;
        gameSo.OnHpChange += UpdateUI;
        gameSo.OnGameOverChange += UpdateUI;
        panelUIinGame.SetActive(true);
        panelStart.SetActive(true);
    }

    private void OnDisable()
    {
        gameSo.OnScoreChange -= UpdateUI;
        gameSo.OnHpChange -= UpdateUI;
        gameSo.OnGameOverChange -= UpdateUI;
    }

    void StartGame()
    {
        panelStart.SetActive(false);
    }

    private void UpdateUI()
    {
        textInGameScore.text = "Score: " + gameSo.Score;
        textHp.text = "HP: " + gameSo.Hp;
        textTimeBetweenSpawn.text = "Time between spawn: " + Math.Round(gameSo.TimeBetweenSpawn, 2) + " s";
        textMultiplier.text = "X" + gameSo.Multiplier;
        textMultiplier.enabled = gameSo.Multiplier > 1;
        


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

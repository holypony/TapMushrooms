using System;
using TMPro;
using UnityEngine;


public class UImanager : MonoBehaviour
{
    [Header("In game UI")]
    [SerializeField] private GameObject panelUIinGame;
    [SerializeField] private TMP_Text textInGameScore;
    [SerializeField] private TMP_Text textHp;
    [SerializeField] private TMP_Text textTimeBetweenSpawn;
    [SerializeField] private TMP_Text textMultiplier;
    [SerializeField] private ParticleSystem psMultiplierEarth;
    [SerializeField] private Animator animatorMultiplierText;
    [Header("Game Over")]
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private TMP_Text textAfterGameScore;
    [SerializeField] private GameObject panelStart;
    [SerializeField] private TMP_Text textBestScore;
    [Space(10)]
    [SerializeField] private GameManagerSo gameSo;


    private void OnEnable()
    {
        gameSo.OnScoreChange += updateScore;
        gameSo.OnHpChange += updateHp;
        gameSo.OnGameOverChange += gameoverUI;
        gameSo.OnMultiplierChange += bonusTime;
        
        panelUIinGame.SetActive(true);
        panelStart.SetActive(true);
    }

    private void OnDisable()
    {
        gameSo.OnScoreChange -= updateScore;
        gameSo.OnHpChange -= updateHp;
        gameSo.OnGameOverChange -= gameoverUI;
        gameSo.OnMultiplierChange -= bonusTime;
    }

    private void bonusTime()
    {
        if (gameSo.Multiplier != 1)
        {
            psMultiplierEarth.Play();
            animatorMultiplierText.SetTrigger("Enable");
            textMultiplier.text = "X" + gameSo.Multiplier;
        }

        if (gameSo.Multiplier != 1) return;
        psMultiplierEarth.Stop();
        if (animatorMultiplierText.GetCurrentAnimatorStateInfo(0).IsName("Multipl_idle"))
        {
            animatorMultiplierText.SetTrigger("Disable");
        }
    }

    private void updateHp()
    {
        textHp.text = "HP: " + gameSo.Hp;
    }

    private void gameoverUI()
    {
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
    
    public void StartGame()
    {
        panelStart.SetActive(false);
    }

    private void updateScore()
    {
        textInGameScore.text = "Score: " + gameSo.Score;
        textTimeBetweenSpawn.text = "TbS: " + Math.Round(gameSo.TimeBetweenSpawn, 2) + " s";
    }
}

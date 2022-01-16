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
    [SerializeField] private TMP_Text textCurrentScore;
    [SerializeField] private TMP_Text textTotalPlayers;
    [Space(10)]
    [SerializeField] private GameManagerSo gameSo;
    [SerializeField] private leaderboardManager leaderboardSo;
    [SerializeField] private TMP_Text[] leadersTexts; 
    private void OnEnable()
    {
        gameSo.OnScoreChange += updateScore;
        gameSo.OnHpChange += updateHp;
        gameSo.OnGameOverChange += gameoverUI;
        gameSo.OnMultiplierChange += bonusTime;

        leaderboardSo.OnValueChange += CreateLeaderboard;
        panelUIinGame.SetActive(true);
        panelStart.SetActive(true);
    }

    private void OnDisable()
    {
        gameSo.OnScoreChange -= updateScore;
        gameSo.OnHpChange -= updateHp;
        gameSo.OnGameOverChange -= gameoverUI;
        gameSo.OnMultiplierChange -= bonusTime;
        
        leaderboardSo.OnValueChange -= CreateLeaderboard;
    }

    private void CreateLeaderboard(bool isUpdated)
    {
        if (isUpdated)
        {
            for (int i = 0; i < leaderboardSo.LeadersList.Count; i++)
            {
                leadersTexts[i].text = leaderboardSo.LeadersList[i];
            }
            textTotalPlayers.text = "Total players: " + leaderboardSo.TotalPlayers;
            leaderboardSo.Value = false;
        }
        
    }

    private void bonusTime(int multiplier)
    {
        if (multiplier != 1)
        {
            psMultiplierEarth.Play();
            animatorMultiplierText.SetTrigger("Enable");
            textMultiplier.text = "X" + gameSo.Multiplier;
        }

        if (multiplier != 1) return;
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

    private void gameoverUI(bool isGameover)
    {
        if (isGameover)
        {
            textTotalPlayers.text = "Total players: ";

            textCurrentScore.text = "Score: " + gameSo.Score;


            textAfterGameScore.text = "";
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

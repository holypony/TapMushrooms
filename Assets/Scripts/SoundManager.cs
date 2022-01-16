using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    
    [SerializeField] private GameManagerSo gameSo;
    [SerializeField] private BoolValueSO isSoundOn;
    [SerializeField] private AudioSource asBg;
    [SerializeField] private AudioSource asSounds;
    
    [Header("Sounds")]
    [SerializeField] private AudioClip soundBonusTime;
    [SerializeField] private AudioClip soundGameOver;
    private void OnEnable()
    {
        isSoundOn.Value = true;
        isSoundOn.OnValueChange += btnSwitchSound;
        gameSo.OnGameOverChange += PlayGameOver;
        gameSo.OnMultiplierChange += PlayBonusTime;
    }
    private void OnDisable()
    {
        isSoundOn.OnValueChange -= btnSwitchSound;
        gameSo.OnGameOverChange -= PlayGameOver;
        gameSo.OnMultiplierChange -= PlayBonusTime;
    }

    private void btnSwitchSound(bool isSound)
    {
        asBg.mute = !isSound;

    }

    private void PlayGameOver(bool isGameOver)
    {
        if (isGameOver)
        {
            asSounds.clip = soundGameOver;
            asSounds.Play();
        }
        
    }
    
    private void PlayBonusTime(int multiplier)
    {
        if (multiplier > 1)
        {
            asSounds.clip = soundBonusTime;
            asSounds.Play();
        }
        
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private BoolValueSO isSoundOn;
    [SerializeField] private AudioSource asBg;
    
    private void OnEnable()
    {
        isSoundOn.OnValueChange += btnSwitchSound;
    }
    private void OnDisable()
    {
        isSoundOn.OnValueChange -= btnSwitchSound;
    }

    private void btnSwitchSound(bool isSound)
    {
        asBg.mute = !isSound;

    }

}

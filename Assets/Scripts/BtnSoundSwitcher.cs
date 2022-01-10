using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnSoundSwitcher : MonoBehaviour
{
    [SerializeField] private BoolValueSO isSoundOn;
    [SerializeField] private Sprite musicOn;
    [SerializeField] private Sprite musicOff;
    [SerializeField] private Image imgBtn;

    public void SwitchSound()
    {
        if (isSoundOn.Value)
        {
            imgBtn.sprite = musicOff;
            isSoundOn.Value = false;
            
        }
        else
        {
            imgBtn.sprite = musicOn;
            isSoundOn.Value = true;
        }
    }

}

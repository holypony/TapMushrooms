using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    
    [SerializeField] private GameManagerSo gameSo;
    [SerializeField] private BoolValueSO isSoundOn;
    [SerializeField] private AudioSource asBg;
    [SerializeField] private AudioSource asSeconsSounds;
    [SerializeField] private AudioSource asMainSounds;
    
    [Header("Sounds")]
    [SerializeField] private AudioClip soundBonusTime;
    [SerializeField] private AudioClip soundGameOver;
    [SerializeField] private AudioClip[] soundOnTap;
    
    
    public static SoundManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(this.gameObject);
    }
    
    public void MushroomTapSound()
    {
        int i = Random.Range(0, soundOnTap.Length);
        asMainSounds.clip = soundOnTap[i];
        asMainSounds.Play();
    }
    
    
    
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
        asSeconsSounds.mute = !isSound;
        asMainSounds.mute = !isSound;

    }

    private void PlayGameOver(bool isGameOver)
    {
        if (isGameOver)
        {
            asSeconsSounds.clip = soundGameOver;
            asSeconsSounds.Play();
        }
        
    }
    
    private void PlayBonusTime(int multiplier)
    {
        if (multiplier > 1)
        {
            asSeconsSounds.clip = soundBonusTime;
            asSeconsSounds.Play();
        }
        
    }

}

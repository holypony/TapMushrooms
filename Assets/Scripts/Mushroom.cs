using System;
using UnityEngine;
using System.Collections;
using MoreMountains.NiceVibrations;
using Random = UnityEngine.Random;

public class Mushroom : MonoBehaviour
{
    
    [SerializeField] private float _lifeTime;
    private bool _isActive = false;
    private bool _clicked = false;
    private bool _isBomb = false;
    
    [SerializeField] private GameManagerSo gameManagerSo;
    [SerializeField] private mushroomsSkinsSO skins;
    
   
    
    [SerializeField] private ParticleSystem psMinusHp;
    [SerializeField] private ParticleSystem psTap;

    private Animator _animator;
    private MeshRenderer _renderer;

    
    
  
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<MeshRenderer>();
    }

    public bool CheckMushroomState()
    {
        return _isActive;
    }

    public void TesterClick(bool killAll = false)
    {
        if (!_isBomb)
        {
            if (_clicked) return;
            _clicked = true;
            SoundManager.instance.MushroomTapSound();
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
            psTap.Play(true);
            StartCoroutine(MushroomDeath());
        }
    }

    public void ShowDie()
    {
        if (_clicked) return;
        _clicked = true;
        SoundManager.instance.MushroomTapSound();
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        psTap.Play(true);
        StartCoroutine(MushroomDeath());
    }
    
    public void ShowHide()
    {
        gameManagerSo.Hp--;
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        psMinusHp.Play(true);
        _animator.SetTrigger("Hide");
    }
    
    public void mushroomState(bool isActive, bool isBomb = false)
    {
        this._isActive = isActive;
        
        if (this._isActive)
        {
            InitMushroom(isBomb);
        }
        else
        {
            psMinusHp.Stop(true);
            StopAllCoroutines();
        }
    }

    private void OnMouseDown()
    {
        if (_clicked) return;
        _clicked = true;
        
        MMVibrationManager.Haptic(HapticTypes.LightImpact);
        psTap.Play(true);
        StartCoroutine(MushroomDeath());
    }

    void InitMushroom(bool isBomb)
    {
        _isBomb = isBomb;
        _clicked = false;
        
        _lifeTime = gameManagerSo.MushLifeTime;
        if (isBomb)
        {
            _lifeTime *= 0.75f;
            _renderer.materials = skins.MushroomsSkinBlack[3].skins;
        }
        else
        {
            int randomSkin = Random.Range(0, 3);
            _renderer.materials = skins.MushroomsSkinBlack[randomSkin].skins;
        }
        
        StartCoroutine(MushroomRoutine());
        _animator.SetTrigger("Start");
    }

    private IEnumerator MushroomRoutine()
    {
        while (_lifeTime > 0)
        {
            if (gameManagerSo.GameOver)
            {
                _animator.SetTrigger("Hide");
                yield return new WaitForSeconds(0.1f);
                mushroomState(false);
                yield break;
            }

            _lifeTime -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        if (!_clicked)
        {
            StartCoroutine(MushroomDeath()); 
        }
    }

    private IEnumerator MushroomDeath()
    {
        _lifeTime = 0;
        if (_clicked)
        {
            if(_isBomb)
            {
                gameManagerSo.BombMushroomsLive--;
                gameManagerSo.Hp--;
                SoundManager.instance.OnLoseHp();
                psMinusHp.Play(true);
                MMVibrationManager.Haptic(HapticTypes.MediumImpact);
                _animator.SetTrigger("Die");
                while (_animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
                {
                    yield return null;
                }
                yield return new WaitForSeconds(0.15f);
                mushroomState(false);
                yield break;
            }

            _animator.SetTrigger("Die");
            while (_animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                yield return null;
            }
            
            SoundManager.instance.MushroomTapSound();
            gameManagerSo.Score += 10 * gameManagerSo.Multiplier;
            yield return new WaitForSeconds(0.15f);
            mushroomState(false);
            yield break;
        }
        
        if (_isBomb)
        {
            gameManagerSo.BombMushroomsLive--;
            _animator.SetTrigger("Hide");
            while (_animator.GetCurrentAnimatorStateInfo(0).IsName("Hide"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.15f);
            mushroomState(false);
            
        }
        else
        {
            SoundManager.instance.OnLoseHp();
            gameManagerSo.Hp--;
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            psMinusHp.Play(true);
            _animator.SetTrigger("Hide");
            while (_animator.GetCurrentAnimatorStateInfo(0).IsName("Hide"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.15f);
            mushroomState(false);
        }
    }
}

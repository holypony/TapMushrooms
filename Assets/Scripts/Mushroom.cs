using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;


public class Mushroom : MonoBehaviour
{
    private float _lifeTime;
    [SerializeField] private bool isActive = false;
    private bool clicked = false;
    private bool isBomb = false;
    
    [SerializeField] private GameManagerSo gameManagerSo;
    [SerializeField] private Material[] matirials;
    [SerializeField] private Material matirialBlack;
    
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
        return isActive;
    }
    
    public void mushroomState(bool _isActive)
    {
        isActive = _isActive;
        
        if (isActive)
        {
            InitMushroom();
        }
        else
        {
            psMinusHp.Stop(true);
            StopAllCoroutines();
        }
    }

    private void OnMouseDown()
    {
        if (clicked) return;
        clicked = true;
        psTap.Play(true);
        StartCoroutine(MushroomDeath());
    }

    void InitMushroom()
    {
        isBomb = false;
        clicked = false;
        _lifeTime = gameManagerSo.MushLifeTime;
        
        if (GetRandom(13))
        {
            isBomb = true;
            _renderer.material = matirialBlack;
        }
        else
        {
            _renderer.material = matirials[Random.Range(0, matirials.Length)];
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

        if (!clicked)
        {
            StartCoroutine(MushroomDeath()); 
        }
    }

    private IEnumerator MushroomDeath()
    {
        _lifeTime = 0;
        if (clicked)
        {
            if(isBomb)
            {
                gameManagerSo.Hp--;
                psMinusHp.Play(true);
                _animator.SetTrigger("Die");
                while (_animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
                {
                    yield return null;
                }
                yield return new WaitForSeconds(0.1f);
                mushroomState(false);
                yield break;
            }

            _animator.SetTrigger("Die");
            while (_animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                yield return null;
            }

            gameManagerSo.Score += 10 * gameManagerSo.Multiplier;
            yield return new WaitForSeconds(0.1f);
            mushroomState(false);
            yield break;
        }
        
        if (isBomb)
        {
            _animator.SetTrigger("Hide");
            while (_animator.GetCurrentAnimatorStateInfo(0).IsName("Hide"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            mushroomState(false);
            
        }
        else
        {
            gameManagerSo.Hp--;
            psMinusHp.Play(true);
            _animator.SetTrigger("Hide");
            while (_animator.GetCurrentAnimatorStateInfo(0).IsName("Hide"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            mushroomState(false);
        }
    }

 
    private bool GetRandom(float setChance)
    {
        var drop = Random.Range(0, 101);
        return drop <= setChance;
    }
}

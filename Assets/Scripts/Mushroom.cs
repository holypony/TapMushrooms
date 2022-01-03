using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;


public class Mushroom : MonoBehaviour
{
    private bool _isActive = false;
    private bool _clicked = false;
    private bool _isBomb = false;
    
    
    [SerializeField] private GameManagerSo gameManagerSo;
    [SerializeField] private Material[] matirials;
    [SerializeField] private Material matirialBlack;
    
    private float _lifeTime;
    private Animator _animator;
    private MeshRenderer _renderer;

    

    public bool IsMushroomDeactivated()
    {
        return _isActive;
    }
    
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<MeshRenderer>();
    }

    public void mushroomState(bool isActive)
    {
        _isActive = isActive;
        
        if (_isActive)
        {
            InitMushroom();
            
        }
        else
        {
            StopAllCoroutines();
        }
    }

    private void OnMouseDown()
    {
        if(!_clicked)
        {
            _clicked = true;
        }
    }

    void InitMushroom()
    {
        _isBomb = false;
        _clicked = false;
        _lifeTime = gameManagerSo.MushLifeTime;
        
        if (GetRandom(gameManagerSo.BombSpawnChance))
        {
            _isBomb = true;
            _renderer.material = matirialBlack;
        }
        else
        {
            _renderer.material = matirials[Random.Range(0, matirials.Length)];
        }
        StartCoroutine(MushroomLife());
        _animator.SetTrigger("Start");
        
    }

    private IEnumerator MushroomLife()
    {
        while (_lifeTime > 0)
        {
            if (_clicked && !_isBomb)
            {
                gameManagerSo.AddScore(10);
                _animator.SetTrigger("Die");
                yield return new WaitForSeconds(0.1f);
                _isActive = false;
                break;
            }

            if (_clicked && _isBomb)
            {
                gameManagerSo.Hp--;
                _animator.SetTrigger("Die");
                yield return new WaitForSeconds(0.1f);
                break;
            }

            if (gameManagerSo.GameOver)
            {
                _animator.SetTrigger("Die");
                _isActive = false;
                yield break;
            }
            _lifeTime -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        
        if (!_clicked)
        {
            if (_isBomb)
            {
                _animator.SetTrigger("Hide");
                yield return new WaitForSeconds(0.1f);
                _isActive = false;
            }
            else
            {
                _animator.SetTrigger("Hide");
                gameManagerSo.Hp--;
                yield return new WaitForSeconds(0.1f);
                _isActive = false;
            }
        }
    }
    
    private bool GetRandom(float setChance)
    {
        var drop = Random.Range(0, 101);
        return drop <= setChance;
    }
}

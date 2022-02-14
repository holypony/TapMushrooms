using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
//using UnityEngine.Rendering.PostProcessing;

public class PostEffects : MonoBehaviour
{

    [SerializeField] Volume m_Volume;
    
    void Start()
    {
        Bloom _bloom;
        if (m_Volume.profile.TryGet<Bloom>(out _bloom))
        {
            _bloom.intensity.value = 5f;
        }
    }
}

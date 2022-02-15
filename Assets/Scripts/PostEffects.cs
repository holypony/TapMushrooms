using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
//using UnityEngine.Rendering.PostProcessing;

public class PostEffects : MonoBehaviour
{

    [SerializeField] Volume m_Volume;
    
   

    public void RandomEffects()
    {
        Bloom _bloom;
        LensDistortion lens;
        
        if (m_Volume.profile.TryGet<Bloom>(out _bloom))
        {
            _bloom.intensity.value = Random.Range(0.65f, 3f);
        }
        
        if (m_Volume.profile.TryGet<LensDistortion>(out lens))
        {
            lens.intensity.value = Random.Range(-0.5f, 0.5f);
        }
        
    }
}

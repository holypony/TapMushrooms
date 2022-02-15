using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spot : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float randomDelayMin  = 0.2f;
    [SerializeField] private float randomDelayMax  = 0.2f;
    [SerializeField] private float staticDelay = 0.2f;
    private void Awake()
    {
        
        StartCoroutine(Levitation());
    }

    private IEnumerator Levitation()
    {
        float randomDelay = Random.Range(randomDelayMin, randomDelayMax);
        yield return new WaitForSeconds(randomDelay + staticDelay);
        animator.enabled = true;
    }

}

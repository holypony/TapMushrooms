using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spot : MonoBehaviour
{
    private float defaultPosY;
    [SerializeField] private float maxLift  = 0.2f;
    [SerializeField] private float minLift = 0.2f;
    private void Start()
    {
        defaultPosY = transform.position.y;
        StartCoroutine(Levitation());
    }

    private IEnumerator Levitation()
    {
        
        yield return new WaitForSeconds(Random.Range(0.1f,0.5f));
        while (true)
        {

            while (transform.position.y >= defaultPosY - minLift)
            {

                transform.position = new Vector3(transform.position.x,transform.position.y - 0.005f, transform.position.z);
                yield return new WaitForSeconds(0.1f);
            }

            while (transform.position.y <= defaultPosY + maxLift)
            {
                transform.position = new Vector3(transform.position.x,transform.position.y + 0.005f, transform.position.z);
                yield return new WaitForSeconds(0.1f);
            }
            
            yield return new WaitForSeconds(0.1f);
        }
    }
}

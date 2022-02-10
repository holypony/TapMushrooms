using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoll : MonoBehaviour
{

    [SerializeField] private float angleZ = 0.1f;
    [SerializeField] private float secondBetweenRotate = 0.1f;
    void Start()
    {
        StartCoroutine(Roll());
    }

    private IEnumerator Roll()
    {
        while (true)
        {
            transform.Rotate(0f, 0f, angleZ);
            yield return new WaitForSeconds(secondBetweenRotate);
            
        }
    }
}

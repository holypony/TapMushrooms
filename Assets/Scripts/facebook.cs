using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
public class facebook : MonoBehaviour
{
    void Awake ()
    {
        if (!FB.IsInitialized) {
            // Initialize the Facebook SDK
            FB.Init();
        } else {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }
}

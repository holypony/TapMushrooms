using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
public class FirebaseAnalytics : MonoBehaviour
{
    private void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
           
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
        
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent("NICE");
        
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public class FirebaseAnalytics : MonoBehaviour
{
    private FirebaseApp app;
    private DatabaseReference reference;
    [SerializeField] private GameManagerSo gameSo;
    
    Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser newUser;

    private void OnEnable()
    {
        gameSo.OnBestScoreChange += AddBestScore;
    }
    
    private void OnDisable()
    {
        gameSo.OnBestScoreChange -= AddBestScore;
    }

    private void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                app = Firebase.FirebaseApp.DefaultInstance;
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                anonymousAuth();
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
        
    }

    private void anonymousAuth()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }
         
            newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

        });
    }

    private void AddBestScore()
    {
        reference.Child("Users").Child(newUser.UserId).Child("BestScore").SetValueAsync(gameSo.BestScore).ContinueWith(task =>
        {
            if (task.IsCompleted) 
            {
                Debug.Log("Db successful updated");
            }
            else
            {
                Debug.Log("db failed");
            }
        });
    }
    
    
    public void AnalyticRestartBtn()
    {
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent("RestartBtn");
    }
}

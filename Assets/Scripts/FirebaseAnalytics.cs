using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class FirebaseAnalytics : MonoBehaviour
{
    
    [SerializeField] private GameManagerSo gameSo;
    [SerializeField] private leaderboardManager leaderboardSo;
    [SerializeField] private Text usernameField;
    
    private DatabaseReference reference;
    private FirebaseAuth auth;
    private FirebaseUser newUser;

    public static FirebaseAnalytics instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(this.gameObject);
       }
    
    private void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {

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

    public void SetDefaultName()
    {
        int randomName = Random.Range(1000, 9999);
        String defaultNick = "mushroom" + randomName;
        StartCoroutine(UpdateUsernameDatabase(defaultNick));
        leaderboardSo.Username = defaultNick;

    }
   
    public void AddBestScore()
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
    
    public void StartGameButton()
    {
        StartCoroutine(LoadUserData());
        UpdateUserDate();
    } 
    
    private IEnumerator UpdateUsernameDatabase(string username)
    {


        //Set the currently logged in user username in the database
        var DBTask = reference.Child("Users").Child(newUser.UserId).Child("UserName").SetValueAsync(username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database username is now updated
        }
    }


    public void UpdateUserDate()
    {
        var date = DateTime.Now.ToString("yyyy-MM-dd");
        reference.Child("Users").Child(newUser.UserId).Child("TimeUpdate").SetValueAsync(date).ContinueWith(task =>
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
        
        reference.Child("Users").Child(newUser.UserId).Child("GamesPlayed").SetValueAsync(leaderboardSo.TotalGamesPlayed).ContinueWith(task =>
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
    private IEnumerator LoadUserData()
    {
        //Get the currently logged in user data
        var DBTask = reference.Child("Users").Child(newUser.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }

        
        DataSnapshot snapshot = DBTask.Result;
        if (!snapshot.Child("UserName").Exists)
        {
            SetDefaultName();
        }
    }
    
    public void AnalyticRestartBtn()
    {
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent("RestartBtn");
    }

    private int yourNumber;
    private int bestFive = 6;

    public void StartScoreboardLoader()
    {
        StartCoroutine(LoadScoreboardData());
    }
    
    private IEnumerator LoadScoreboardData()
    {
        if (gameSo.GameOver)
        {
            var DBTask = reference.Child("Users").OrderByChild("BestScore").GetValueAsync();
            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                leaderboardSo.LeadersList.Clear();
                DataSnapshot snapshot = DBTask.Result;
                int i = 0;
                foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
                {
                    i++;
                    string userId = childSnapshot.Key;
                    int bestScore = int.Parse(childSnapshot.Child("BestScore").Value.ToString());
                    
                    if ( bestFive > i)
                    {
                        if (userId == newUser.UserId && i < 6)
                        {
                            leaderboardSo.LeadersList.Add(i  + ". " + bestScore + " You here!");
                        }

                        if (userId != newUser.UserId && i < 6)
                        {
                            leaderboardSo.LeadersList.Add(i + ". " + bestScore);
                        }
                    }
                    if (userId == newUser.UserId && i >= 6)
                    {
                        leaderboardSo.LeadersList.Add(i  + ". " + bestScore + " You here!");
                    }
                
                    leaderboardSo.TotalPlayers = i;
                }
                leaderboardSo.Value = true;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using Random = UnityEngine.Random;
using Google;
public class FirebaseAnalytics : MonoBehaviour
{
    
    [SerializeField] private GameManagerSo gameSo;
    [SerializeField] private leaderboardManager leaderboardSo;
    private String webClientId  = "276560292292-7dfsv30sgh8r8ebf36eq3ggos7isukaf.apps.googleusercontent.com";
    private DatabaseReference reference;
    private FirebaseAuth auth;
    private FirebaseUser newUser;
    private DependencyStatus dependencyStatus;
    private GoogleSignInConfiguration configuration;

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
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    public void SignInWithGoogle() { OnSignIn(); }
    
    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        //AddToInformation("Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }
    
    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.Log("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.Log("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.Log("Canceled");
        }
        else
        {
            Debug.Log("Welcome: " + task.Result.DisplayName + "!");
            Debug.Log("Email = " + task.Result.Email);
            Debug.Log("Google ID Token = " + task.Result.IdToken);
            Debug.Log("Email = " + task.Result.Email);
            SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }
    
    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    Debug.Log("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                newUser = task.Result;
                Debug.Log("Sign In Successful.");
            }
        });
    }
    private void OnEnable()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;

        gameSo.OnBestScoreChange += AddBestScore;
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token) 
    {
        Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e) 
    {
        Debug.Log("Received a new message from: " + e.Message.From);
    }
    
    private void InitializeFirebase()
    {

        auth = FirebaseAuth.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        
        newUser = auth.CurrentUser;
        if (newUser != null)
        {
            string name = newUser.DisplayName;
            string email = newUser.Email;
            System.Uri photo_url = newUser.PhotoUrl;
            // The user's Id, unique to the Firebase project.
            // Do NOT use this value to authenticate with your backend server, if you
            // have one; use User.TokenAsync() instead.
            string uid = newUser.UserId;
        }
        else
        {
            anonymousAuth(); 
        }
        //Debug.Log("Firebase ____ email" + newUser.Email);
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
    
    
    public void StartGameButton()
    {
        if (newUser == null)
        {
            //anonymousAuth();
        }
        
        if(reference == null) return;
        UpdateUserLastOnline();
        StartCoroutine(CheckUserName());
        
        FirstStart();
    }

    private void FirstStart()
    {
        var i = PlayerPrefs.GetInt("isFirstRun", 1);
        if(i == 0) return;
        
        UpdateUserRegTime();
        UpdateUserLang();
        UpdateUserDevice();
        
        PlayerPrefs.SetInt("isFirstRun", 0);
    }
    
    private void UpdateUserLastOnline()
    {
        var date = DateTime.Now.ToString("yyyy-MM-dd\\THH:mm:ss\\Z");
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
    }
    
    private void UpdateUserRegTime()
    {
        
        var date = DateTime.Now.ToString("yyyy-MM-dd\\THH:mm:ss\\Z");
        reference.Child("Users").Child(newUser.UserId).Child("RegistrationTime").SetValueAsync(date).ContinueWith(task =>
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
    
    private void UpdateUserLang()
    {
        var lang = Application.systemLanguage.ToString();
        reference.Child("Users").Child(newUser.UserId).Child("Language").SetValueAsync(lang).ContinueWith(task =>
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
    
    public void UpdateAdsShown(int AdsShown)
    {
        reference.Child("Users").Child(newUser.UserId).Child("AdsShown").SetValueAsync(AdsShown).ContinueWith(task =>
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
    
    private void UpdateUserDevice()
    {
        var deviceModel = SystemInfo.deviceModel + " QualityLevel: "+ QualitySettings.GetQualityLevel();
        reference.Child("Users").Child(newUser.UserId).Child("Device model").SetValueAsync(deviceModel).ContinueWith(task =>
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
    
    private IEnumerator CheckUserName()
    {
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
        else
        {
            leaderboardSo.Username = ""+ snapshot.Child("UserName").Value;
        }

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
        if(reference == null) return;
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

    private IEnumerator UpdateUsernameDatabase(string username)
    {
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
    
    public void UpdateTotalGames()
    {
        if(reference == null) return;
        int i = PlayerPrefs.GetInt("totalGamePlayed", 1);
        reference.Child("Users").Child(newUser.UserId).Child("GamesPlayed").SetValueAsync(i).ContinueWith(task =>
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

    private int yourNumber;
    private int bestFive = 6;

    public void StartScoreboardLoader()
    {
        if(reference == null) return;
        StartCoroutine(LoadScoreboardData());
    }
    
    private IEnumerator LoadScoreboardData()
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
                if (!childSnapshot.Child("BestScore").Exists) continue;
                
                i++;
                string userId = childSnapshot.Key;
                int bestScore = int.Parse(childSnapshot.Child("BestScore").Value.ToString());
                
                if (bestFive > i)
                {
                    if (userId == newUser.UserId && i < 6)
                    {
                        leaderboardSo.LeadersList.Add(i + ". " + bestScore + " Your score");
                    }

                    if (userId != newUser.UserId && i < 6)
                    {
                        leaderboardSo.LeadersList.Add(i + ". " + bestScore);
                    }
                }

                if (userId == newUser.UserId && i >= 6)
                {
                    
                    leaderboardSo.LeadersList.Add(i + ". " + bestScore + " Your score");
                }
            }
            Debug.Log(i);
            leaderboardSo.TotalPlayers = i;
            leaderboardSo.Value = true;
        }
    }
}

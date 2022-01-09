using System;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class datebase : MonoBehaviour
{
    private DatabaseReference reference;
    [SerializeField] private GameManagerSo gameSo;
    private void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        
    }

    public void saveData()
    {
        string json = JsonUtility.ToJson(gameSo.BestScore);
        if (reference != null)
        {
            reference.Child("Score").SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsCompleted) 
                {
                    Debug.Log("Db succsesfully updated");
                }
                else
                {
                    Debug.Log("db failed");
                }
            });
        }
        
    }
}

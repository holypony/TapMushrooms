using System;
using UnityEngine;

[CreateAssetMenu(fileName = "mrSO", menuName = "Data/MashRoomSO")]

public class mashroomSO : ScriptableObject
{
    [SerializeField] private bool isActive;

    [SerializeField] private int minLifeTime;
    [SerializeField] private int maxLifeTime;



    public event Action<bool> OnMashroomActivate;
    public bool IsActive
    {
        get => isActive;
        set
        {
            isActive = value;
            OnMashroomActivate?.Invoke(isActive);
        }

    }




   

}
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BoolData")]
public class BoolValueSO : ScriptableObject
{
    [SerializeField]
    private bool _value;

    public bool Value
    {
        get => _value;
        set
        {
            _value = value;
            OnValueChange?.Invoke(_value);
        }

    }
    public event Action<bool> OnValueChange;

}
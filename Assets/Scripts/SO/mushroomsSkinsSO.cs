using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/mushroomsSkins")]
public class mushroomsSkinsSO : ScriptableObject
{
    [Serializable]
    public class Skins
    {
        public Material[] skins;
    }
    

    public Skins[] MushroomsSkinBlack;
}

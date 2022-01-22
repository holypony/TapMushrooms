using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour
{

    [SerializeField] private String privacyPolicyLink = "https://telegra.ph/Privacy-Policy-01-02-36";
    [SerializeField] private String googlePlayLink = "https://play.google.com/store/apps/details?id=com.tapmushrooms";
    public void btnRayeUs()
    {
        Application.OpenURL(googlePlayLink);
    }
    
    public void btnPrivacyPolicy()
    {
        Application.OpenURL(privacyPolicyLink);
    }
}

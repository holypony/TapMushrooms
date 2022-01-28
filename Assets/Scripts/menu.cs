using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class menu : MonoBehaviour
{

    [SerializeField] private String privacyPolicyLink = "https://telegra.ph/Privacy-Policy-01-02-36";
    [SerializeField] private String termsOfUseLink = "https://telegra.ph/Terms--Conditions-01-25";
    [SerializeField] private String googlePlayLink = "https://play.google.com/store/apps/details?id=com.tapmushrooms";
    [SerializeField] private TMP_Text textQuality;
    public void btnRayeUs()
    {
        Application.OpenURL(googlePlayLink);
    }
    
    public void btnPrivacyPolicy()
    {
        Application.OpenURL(privacyPolicyLink);
    }


    public void Set0q()
    {
        textQuality.text = QualitySettings.GetQualityLevel() + " quality";
        Debug.Log(  QualitySettings.GetQualityLevel() +  "  QualitySettings before");
        QualitySettings.SetQualityLevel(0);
        Debug.Log(  QualitySettings.GetQualityLevel() +  "  QualitySettings after");
        
    }
    
    public void Set1q()
    {
        textQuality.text = QualitySettings.GetQualityLevel() + " quality";
        Debug.Log(  QualitySettings.GetQualityLevel() +  "  QualitySettings before");
        QualitySettings.SetQualityLevel(1);
        Debug.Log(  QualitySettings.GetQualityLevel() +  "  QualitySettings after");
    }
    
    public void Set2q()
    {
        textQuality.text = QualitySettings.GetQualityLevel() + " quality";
        Debug.Log(  QualitySettings.GetQualityLevel() +  "  QualitySettings before");
        QualitySettings.SetQualityLevel(2);
        Debug.Log(  QualitySettings.GetQualityLevel() +  "  QualitySettings after");
    }
}

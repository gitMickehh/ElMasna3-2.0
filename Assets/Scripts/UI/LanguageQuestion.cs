using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageQuestion : MonoBehaviour
{
    public GameConfig configFile;

    [Header("Langugae Profiles")]
    public LanguageProfile AR;
    public LanguageProfile EN;

    public void PlaySound()
    {
        AudioManager.instance.Play("Swish");
    }

    public void ChangeLanguage(int index)
    {
        if(index == 1)
        {
            //arabic
            configFile.CurrentLanguageProfile = AR;
        }
        else
        {
            //english
            configFile.CurrentLanguageProfile = EN;
        }
    }
}

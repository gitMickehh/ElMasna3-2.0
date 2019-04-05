using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveHome : MonoBehaviour
{

    [Header("Game Config File")]
    public GameConfig GameConfigFile;

    //#if UNITY_EDITOR
    [Header("Editor Options")]
    public bool save;
    public bool load;
    public bool focusSave;
    //#endif

    public UI_OptionsMenu uI_OptionsMenu;

    public bool loadedAll;

    private void Start()
    {
        loadedAll = false;

        //#if UNITY_EDITOR
        if (load)
        {
            //#endif
            LoadAll();
            //#if UNITY_EDITOR
        }
        //#endif
    }

    private void SaveSounds()
    {

    }

    private void LoadSounds()
    {

    }

    private void SaveLanguage()
    {
        var cLanguage = GameConfigFile.CurrentLanguageProfile.name;
        //var languageString = JsonUtility.ToJson(cLanguage);

        //Debug.Log(languageString);
        //PlayerPrefs.SetString("language", languageString);

        Debug.Log(cLanguage);
        PlayerPrefs.SetString("language", cLanguage);
    }

    private void LoadLanguage()
    {
        if (PlayerPrefs.HasKey("language"))
        {
            string languageString = /*JsonUtility.FromJson<string>*/(PlayerPrefs.GetString("language"));
            Debug.Log("languageString: " + languageString);

            if (languageString != "")
            {
                int index;
                index = (int)Enum.Parse(typeof(Languages), languageString);

                Debug.Log("index: " + index);
                uI_OptionsMenu.ChangeLanguage(index);
            }
            //PlayerPrefs.DeleteKey("language");
        }
        else
        {
            Debug.LogWarning("Language not saved!");
        }
    }

    private void LoadAll()
    {
        Debug.Log("Loading all");

        LoadLanguage();
        loadedAll = true;
    }

    private void SaveAll()
    {
        Debug.Log("Saving all");
        if (!loadedAll)
            return;

        SaveLanguage();

    }

   

    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        //#if UNITY_EDITOR
        if (save)
        {
            //#endif
            Debug.Log("loadedAll: " + loadedAll);
            if (loadedAll)
                SaveAll();
            //#if UNITY_EDITOR
        }
        //#endif
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focusSave)
            return;

        if (!focus)
        {
            //#if UNITY_EDITOR
            if (save)
            {
                //#endif
                if (loadedAll)
                    SaveAll();
                //#if UNITY_EDITOR
            }
            //#endif
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Globalization;

public class State
{
    public CultureInfo Result { get; set; }
}

public class SaveHome : MonoBehaviour
{

    [Header("Game Config File")]
    public GameConfig GameConfigFile;
    public ActiveSounds activeSounds;

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
        StartCoroutine(LateStart());
        
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();

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
        string soundOn = activeSounds.soundIsOn.ToString();
        string soundFXOn = activeSounds.soundFxIsOn.ToString();

        PlayerPrefs.SetString("soundOn", soundOn);
        PlayerPrefs.SetString("soundFXOn", soundFXOn);
    }

    private void LoadSounds()
    {
        if (PlayerPrefs.HasKey("soundOn"))
        {
            string soundOn = (PlayerPrefs.GetString("soundOn"));
            if(soundOn != "")
            {
                bool sound = bool.Parse(soundOn);

                if(sound)
                    uI_OptionsMenu.sound.isOn = true;
                else
                    uI_OptionsMenu.sound.isOn = false;

                activeSounds.soundIsOn = sound;
                Debug.Log("SoundIsOn: " + sound);
                uI_OptionsMenu.CheckSoundState(Sounds.Sound.ToString());
            }
        }
        else
        {
            Debug.LogWarning("Game Sound not saved!");
        }

        if (PlayerPrefs.HasKey("soundFXOn"))
        {
            string soundFXOn = (PlayerPrefs.GetString("soundFXOn"));
            if (soundFXOn != "")
            {
                bool soundFx = bool.Parse(soundFXOn);

                if (soundFx)
                    uI_OptionsMenu.soundFx.isOn = true;
                else
                    uI_OptionsMenu.soundFx.isOn = false;

                activeSounds.soundFxIsOn = soundFx;
                Debug.Log("soundFxIsOn: " + soundFx);

                uI_OptionsMenu.CheckSoundState(Sounds.SoundFX.ToString());
            }
        }
        else
        {
            Debug.LogWarning("Game SoundFXs not saved!");
        }
    }

    private void SaveLanguage()
    {
        var cLanguage = GameConfigFile.CurrentLanguageProfile.name;

        //Debug.Log(cLanguage);
        PlayerPrefs.SetString("language", cLanguage);
    }

    private void LoadLanguage()
    {
        //PlayerPrefs.DeleteKey("language");

        if (PlayerPrefs.HasKey("language"))
        {
            string languageString = (PlayerPrefs.GetString("language"));
            Debug.Log("languageString: " + languageString);

            if (languageString != "")
            {
                int index;
                index = (int)Enum.Parse(typeof(Languages), languageString);

                Debug.Log("index: " + index);
                uI_OptionsMenu.ChangeLanguage(index);
            }
        }
        else
        {
            string currentLang;
            switch(Application.systemLanguage)
            {
                case SystemLanguage.Arabic:
                case SystemLanguage.English:
                case SystemLanguage.French:
                    currentLang = Application.systemLanguage.ToString().Substring(0, 2).ToUpper();
                    break;
                case SystemLanguage.German:
                    currentLang = "GR";
                    break;

                default:
                    currentLang = "EN";
                    break;
            }

            Debug.LogError("currentLang: " + currentLang);
            
            if (Enum.IsDefined(typeof(Languages), currentLang))
            {
                int index;
                index = (int)Enum.Parse(typeof(Languages), currentLang);
                uI_OptionsMenu.ChangeLanguage(index);
            }
            
            Debug.LogWarning("Language not saved!");
        }
    }

    private void LoadAll()
    {
        Debug.Log("Loading all");

        LoadLanguage();
        LoadSounds();
        loadedAll = true;
    }

    public void SaveAll()
    {
        //Debug.Log("Saving all");
        if (!loadedAll)
            return;

        SaveLanguage();
        SaveSounds();

    }


    private void OnApplicationQuit()
    {
        //#if UNITY_EDITOR
        if (save)
        {
            //#endif
            //Debug.Log("loadedAll: " + loadedAll);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public enum Languages
{
    EN, AR, FR, GR
}
public class UI_OptionsMenu : MonoBehaviour
{
    public GameConfig gameConfigFile;
    public ActiveSounds activeSounds;
    
    public SaveHome saveHome;

    //Modal Panel
    private ModalPanel modalPanel;
    private UnityAction clearSaveAction;

    [Header("Options Panel")]
    public ScriptableObjectsList LanguagesList;

    [Header("Sound Toggles")]
    public Toggle sound;
    public Toggle soundFx;

    public ToggleGroup languagesGroup;
    public List <Toggle> toggles;

    private void OnEnable()
    {
        toggles = languagesGroup.GetComponentsInChildren<Toggle>().ToList<Toggle>();
    }

    private void Start()
    {
        modalPanel = ModalPanel.Instance();
        clearSaveAction = new UnityAction(ClearSaves);

        for (int i =0; i<toggles.Count; i++)
        {
            toggles[i].onValueChanged.AddListener(
                delegate { LanguageChanged(); }
                );
        }
        //LanguageChanged();
        SetActiveToggle();

        sound.onValueChanged.AddListener(delegate
        {
            CheckSoundState(Sounds.Sound.ToString());
        });

        soundFx.onValueChanged.AddListener(delegate
        {
            CheckSoundState(Sounds.SoundFX.ToString());
        });
    }

    public void ClearSaveQuestion()
    {
        string s = gameConfigFile.CurrentLanguageProfile.AreYouSureYouWantToRestartTheGame + gameConfigFile.CurrentLanguageProfile.QuestionMark;
        modalPanel.Choice(s, clearSaveAction);
    }

    public void ChangeLanguage(int i)
    {
        gameConfigFile.CurrentLanguageProfile = (LanguageProfile)LanguagesList.ListElements[i];
    }

    private void ClearSaves()
    {
        PlayerPrefs.DeleteAll();
        FindObjectOfType<LevelLoader>().LoadLevel(1);
    }

    public Toggle ActiveToggle(ToggleGroup toggleGroup)
    {
        return toggleGroup.ActiveToggles().FirstOrDefault();
    }

    public void SetActiveToggle()
    {
        if (ActiveToggle(languagesGroup).name == gameConfigFile.CurrentLanguageProfile.name)
               return;

        var toggle = toggles.Find(item => item.name == gameConfigFile.CurrentLanguageProfile.name);
        toggle.isOn = true;
    }

    public void LanguageChanged()
    {
        Toggle activeToggle = ActiveToggle(languagesGroup);

        if (toggles.Contains(activeToggle))
            ChangeLanguage(toggles.IndexOf(activeToggle));

        saveHome.SaveAll();

    }
    public void CheckSoundState(string soundStr)
    {
        switch (soundStr)
        {
            case "Sound":
                if (sound.isOn)
                    AudioManager.instance.Play("Theme Song");
                else if (!sound.isOn)
                    AudioManager.instance.StopSound("Theme Song");

                activeSounds.soundIsOn = sound.isOn;

                break;

            case "SoundFX":
                if (soundFx.isOn)
                    AudioManager.instance.PlayFXSounds();
                else if (!soundFx.isOn)
                    AudioManager.instance.StopFXSounds();

                activeSounds.soundFxIsOn = soundFx.isOn;
                break;
        }

        saveHome.SaveAll();
    }
}

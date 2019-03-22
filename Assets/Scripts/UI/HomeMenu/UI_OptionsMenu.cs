using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_OptionsMenu : MonoBehaviour
{
    public GameConfig gameConfigFile;

    //Modal Panel
    private ModalPanel modalPanel;
    private UnityAction clearSaveAction;

    [Header("Options Panel")]
    public ScriptableObjectsList LanguagesList;


    private void Start()
    {
        modalPanel = ModalPanel.Instance();
        clearSaveAction = new UnityAction(ClearSaves);
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
}

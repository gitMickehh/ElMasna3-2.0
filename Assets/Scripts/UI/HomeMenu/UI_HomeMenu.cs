using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_HomeMenu : MonoBehaviour
{
    public GameConfig gameConfigFile;

    //Modal Panel
    private ModalPanel modalPanel;
    private UnityAction clearSaveAction;

    private void Start()
    {
        modalPanel = ModalPanel.Instance();
        clearSaveAction = new UnityAction(ClearSaves);
    }

    public void ClearSaveQuestion()
    {
        string s = gameConfigFile.CurrentLanguageProfile.AreYouSure + gameConfigFile.CurrentLanguageProfile.QuestionMark;
        modalPanel.Choice(s, clearSaveAction);
    }

    private void ClearSaves()
    {
        PlayerPrefs.DeleteAll();
    }
}

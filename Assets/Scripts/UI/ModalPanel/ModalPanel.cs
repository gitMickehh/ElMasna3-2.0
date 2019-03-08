using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModalPanel : MonoBehaviour
{
    public GameConfig ConfigFile;

    [Header("UI")]
    public Text question;
    public Image iconImage;
    public Button yesButton;
    public Button cancelButton;
    public GameObject modalPanelObject;

    private static ModalPanel modalPanel;

    public static ModalPanel Instance()
    {
        if (!modalPanel)
        {
            modalPanel = FindObjectOfType(typeof(ModalPanel)) as ModalPanel;

            if (!modalPanel)
                Debug.LogError("There needs to be one active ModalPanel script on a GameObject in your scene. ");
        }

        return modalPanel;
    }

    public void Choice(string question, UnityAction yesEvent, UnityAction cancelEvent)
    {
        modalPanelObject.SetActive(true);

        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(yesEvent);
        yesButton.onClick.AddListener(ClosePanel);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(cancelEvent);
        cancelButton.onClick.AddListener(ClosePanel);

        this.question.text = question;

        this.iconImage.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);

    }

    public void Choice(string question, UnityAction yesEvent)
    {
        modalPanelObject.SetActive(true);

        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(yesEvent);
        yesButton.onClick.AddListener(ClosePanel);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(ClosePanel);

        this.question.text = question;

        this.iconImage.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);

    }

    void ClosePanel()
    {
        modalPanelObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModalPanel : MonoBehaviour
{
    public GameConfig ConfigFile;
    public GameEvent ButtonSoundEvent;
    public GameEvent UIOpenedEvent;
    public GameEvent UIClosedButton;

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

    private void CheckLanguague()
    {
        if(ConfigFile.CurrentLanguageProfile.LeftToRight)
        {
            //english, french, etc..
            yesButton.transform.SetAsFirstSibling();
        }
        else
        {
            //arabic.
            yesButton.transform.SetAsLastSibling();
        }
    }

    public void Choice(string question, UnityAction yesEvent, UnityAction cancelEvent)
    {
        UIOpenedEvent.Raise();
        modalPanelObject.SetActive(true);
        CheckLanguague();

        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(yesEvent);
        yesButton.onClick.AddListener(RaiseButtonSound);
        yesButton.onClick.AddListener(ClosePanel);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(cancelEvent);
        cancelButton.onClick.AddListener(RaiseButtonSound);
        cancelButton.onClick.AddListener(ClosePanel);

        this.question.text = question;

        this.iconImage.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);

    }

    public void Choice(string question, UnityAction yesEvent, UnityAction cancelEvent, Sprite messageIcon = null)
    {
        UIOpenedEvent.Raise();
        modalPanelObject.SetActive(true);
        CheckLanguague();

        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(yesEvent);
        yesButton.onClick.AddListener(RaiseButtonSound);
        yesButton.onClick.AddListener(ClosePanel);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(cancelEvent);
        cancelButton.onClick.AddListener(RaiseButtonSound);
        cancelButton.onClick.AddListener(ClosePanel);

        this.question.text = question;

        if (messageIcon == null)
            iconImage.gameObject.SetActive(false);
        else
        {
            iconImage.sprite = messageIcon;
            iconImage.gameObject.SetActive(true);
        }

        iconImage.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);

    }

    public void Choice(string question, UnityAction yesEvent, Sprite messageIcon = null)
    {
        UIOpenedEvent.Raise();
        modalPanelObject.SetActive(true);
        CheckLanguague();

        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(yesEvent);
        yesButton.onClick.AddListener(RaiseButtonSound);
        yesButton.onClick.AddListener(RaiseUIClose);
        yesButton.onClick.AddListener(ClosePanel);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(RaiseButtonSound);
        cancelButton.onClick.AddListener(RaiseUIClose);
        cancelButton.onClick.AddListener(ClosePanel);

        this.question.text = question;

        if (messageIcon == null)
            iconImage.gameObject.SetActive(false);
        else
        {
            iconImage.sprite = messageIcon;
            iconImage.gameObject.SetActive(true);
        }

        yesButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);

    }

    public void Message(string ConfirmationMessage,Sprite messageIcon = null)
    {
        AudioManager.instance.Play("Notify");
        UIOpenedEvent.Raise();
        modalPanelObject.SetActive(true);
        CheckLanguague();

        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(RaiseButtonSound);
        yesButton.onClick.AddListener(RaiseUIClose);
        yesButton.onClick.AddListener(ClosePanel);

        question.text = ConfirmationMessage;

        if(messageIcon == null)
            iconImage.gameObject.SetActive(false);
        else
        {
            iconImage.sprite = messageIcon;
            iconImage.gameObject.SetActive(true);
        }

        yesButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(false);

    }

    public void AddToYesButton(UnityAction yesButtonAction)
    {
        yesButton.onClick.AddListener(yesButtonAction);
    }

    void RaiseButtonSound()
    {
        ButtonSoundEvent.Raise();
    }

    void RaiseUIClose()
    {
        UIClosedButton.Raise();
    }

    void ClosePanel()
    {
        modalPanelObject.SetActive(false);
    }

}

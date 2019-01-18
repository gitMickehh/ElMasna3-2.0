using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class NewGameSetUp : MonoBehaviour {

    [Tooltip("0 EN, 1 AR, 2 FR, etc...")]
    public LanguageProfile[] Langauges;

    public GameConfig GameConfigFile;

    [Header("Language")]
    public GameObject ConfirmationPanel;
    public Text ConfirmationText;

    [Header("Player Customizaiton")]
    public Button GenderConfirmationButton;
    public GameObject GenderSelectionPanel;
    public InputField PlayerNameInput;
    string playerName;
    Gender playerGender;
    public GameObject MalePrefab;
    public GameObject FemalePrefab;

    int selectedProfile;

    void ShowConfirmationPage(string confirmationText)
    {
        ConfirmationPanel.gameObject.SetActive(true);
        ConfirmationText.text = confirmationText;
    }

    public void EnglishButton()
    {
        ShowConfirmationPage(Langauges[0].ConfirmationText);
        selectedProfile = 0;
    }

    public void ArabicButton()
    {
        ShowConfirmationPage(Langauges[1].ConfirmationText);
        selectedProfile = 1;
    }

    public void FrenchButton()
    {
        ShowConfirmationPage(Langauges[2].ConfirmationText);
        selectedProfile = 2;
    }

    public void ConfirmLanguage()
    {
        GameConfigFile.CurrentLanguageProfile = Langauges[selectedProfile];
    }

    public void CancelConfirmation()
    {
        ConfirmationPanel.gameObject.SetActive(false);
    }

    public void PickGender(int g)
    {
        playerGender = (Gender)g;
        GenderConfirmationButton.interactable = true;
    }

    public void ConfirmGender()
    {
        //GameConfigFile.PlayerGender = playerGender;
        switch (playerGender)
        {
            case Gender.MALE:
                //GameConfigFile.PlayerPrefab = MalePrefab;
                break;
            case Gender.FEMALE:
                //GameConfigFile.PlayerPrefab = FemalePrefab;
                break;
            default:
                //GameConfigFile.PlayerPrefab = MalePrefab;
                break;
        }

        GenderSelectionPanel.SetActive(false);
        PlayerNameInput.placeholder.GetComponent<Text>().text = GameConfigFile.CurrentLanguageProfile.PlaceholderNameText;
    }

    public void ConfirmPlayerName()
    {
        //GameConfigFile.PlayerName = PlayerNameInput.text;
    }

}

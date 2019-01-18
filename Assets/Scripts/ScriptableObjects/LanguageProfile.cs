using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Language Profile", menuName = "ElMasna3/Language Profile")]
public class LanguageProfile : ScriptableObject {

    [Header("Game Title")]
    public string GameName;
    public string Language;
    public string LanguageInitials;
    [TextArea]
    public string ConfirmationText;
    public string PlaceholderNameText;

    [Header("Main Menu")]
    public string StartGame;
    public string QuitGame;
    public string Options;
    public string HowToPlay;

    [Header("InGame")]
    public string Pause;
    public Font MainFont;

    [Header("Names")]
    public string[] MaleNames;
    public string[] FemaleNames;
    public string[] LastNames;
	
}

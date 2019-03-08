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
    public ListOfStrings MaleNames;
    public ListOfStrings FemaleNames;
    public ListOfStrings LastNames;

    [Header("Name Left to Right")]
    [Tooltip("This is true if the name is written from left to right")]
    public bool LeftToRight = true;

    [Header("Quesitons")]
    public string QuestionMark;
    [TextArea]
    public string HireWorker;
    [TextArea]
    public string GiveWorkerBreak;
    [TextArea]
    public string AreYouSure;

    public string GetRandomFullName(Gender g)
    {
        int no1 = Random.Range(0, MaleNames.strings.Count);
        int no2 = Random.Range(0, LastNames.strings.Count);


        if (LeftToRight)
        {
            if(g == Gender.MALE)
                return MaleNames.strings[no1] + " " + LastNames.strings[no2];
            else
                return FemaleNames.strings[no1] + " " + LastNames.strings[no2];
        }
        else
        {
            if(g == Gender.MALE)
                return LastNames.strings[no1] + " " + MaleNames.strings[no2];
            else
                return LastNames.strings[no1] + " " + FemaleNames.strings[no2];

        }
    }

}

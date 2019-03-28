using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Language Profile", menuName = "ElMasna3/Language Profile")]
public class LanguageProfile : ScriptableObject
{

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

    [Header("Modal Panel Statements")]
    public string QuestionMark;
    [TextArea]
    public string HireWorker;
    [TextArea]
    public string GiveWorkerBreak;
    [TextArea]
    public string AreYouSure;
    [TextArea]
    public string NoPlaceForWorkers;
    [TextArea]
    public string YouWillPay;
    [TextArea]
    public string NotEnoughMoney;
    [TextArea]
    public string AreYouSureYouWantToRestartTheGame;
    [TextArea]
    public string DoYouWantToStartTheParty;
    [TextArea]
    public string DoYouWantToBuildANewFloor;

    [Header("Tutorial Statements")]
    public string ExeclamationMark;
    [TextArea]
    public string WelcomeText;

    public string GetRandomFullName(Gender g)
    {
        int no1;

        if (g == Gender.MALE)
            no1 = Random.Range(0, MaleNames.strings.Count);
        else
            no1 = Random.Range(0, FemaleNames.strings.Count);

        int no2 = Random.Range(0, LastNames.strings.Count);


        if (LeftToRight)
        {
            if (g == Gender.MALE)
                return MaleNames.strings[no1] + " " + LastNames.strings[no2];
            else
                return FemaleNames.strings[no1] + " " + LastNames.strings[no2];
        }
        else
        {
            if (g == Gender.MALE)
                return LastNames.strings[no2] + " " + MaleNames.strings[no1];
            else
                return LastNames.strings[no2] + " " + FemaleNames.strings[no1];
        }
    }

    public string GetQuestion(string q)
    {
        string question;

        if (LeftToRight)
        {
            question = q + QuestionMark;
        }
        else
        {
            question = QuestionMark + q;
        }

        return question;

    }

    public string GetQuestion(string[] qs)
    {
        string question;

        if (LeftToRight)
        {
            string bigS = "";

            for (int i = 0; i < qs.Length; i++)
            {
                bigS += qs[i] + " ";
            }

            question = bigS + QuestionMark;
        }
        else
        {
            string bigS = "";

            for (int i = qs.Length - 1; i >= 0; i--)
            {
                bigS += qs[i] + " ";
            }

            question = QuestionMark + bigS;
        }

        return question;

    }

    public string GetStatement(string[] s)
    {
        string statement;

        if (LeftToRight)
        {
            string bigS = "";

            for (int i = 0; i < s.Length; i++)
            {
                bigS += s[i] + " ";
            }

            statement = bigS + ExeclamationMark;
        }
        else
        {
            string bigS = "";

            for (int i = s.Length - 1; i >= 0; i--)
            {
                bigS += s[i] + " ";
            }

            statement = ExeclamationMark + bigS;
        }

        return statement;

    }

}

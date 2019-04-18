using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayManager : MonoBehaviour
{
    public Text displayTextOrigin;
    public Text displayRealMoneyText;
    public Text displayHappyMoneyText;

    public float displayTime;
    public float fadeTime;

    public float timeOfTravel;

    private IEnumerator fadeAlpha;

    private static DisplayManager displayManager;

    public static DisplayManager Instance()
    {
        if (!displayManager)
        {
            displayManager = FindObjectOfType(typeof(DisplayManager)) as DisplayManager;

            if (!displayManager)
                Debug.LogError("There needs to be one active DisplayManager script on a GameObject in your scene. ");

        }

        return displayManager;
    }

    public void DisplayMessage(string message, Text displayText)
    {
        displayText.gameObject.SetActive(true);
        displayText.text = message;
        SetAlpha(displayText);
    }

    public Transform realMoneyPos;
    public Transform happyMoneyPos;
    [SerializeField]

    void SetAlpha(Text displayText)
    {
        if(fadeAlpha != null)
        {
            StopCoroutine(fadeAlpha);
        }

        fadeAlpha = FadeAlpha(displayText);
        StartCoroutine(fadeAlpha);

    }

    IEnumerator FadeAlpha(Text displayText)
    {
        Color resetColor = displayText.color;
        resetColor.a = 1;
        displayText.color = resetColor;

        yield return new WaitForSeconds(displayTime);

        while(displayText.color.a > 0)
        {
            Color displayColor = displayText.color;
            displayColor.a -= Time.deltaTime / fadeTime;
            displayText.color = displayColor;
            yield return null;
        }
        yield return null;
    }
}

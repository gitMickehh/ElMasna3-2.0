using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayManager : MonoBehaviour
{
    public Text displayText;
    public Text displayRealMoneyText;
    public Text displayHappyMoneyText;

    public float displayTime;
    public float fadeTime;

    public float timeOfTravel; //time after object reach a target place 
    float currentTime = 0; // actual floting time 
    float normalizedValue;

    RectTransform rectTransform;

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

    public void DisplayMessage(string message)
    {
        displayText.gameObject.SetActive(true);
        displayText.text = message;
        SetAlpha();
    }

    public Transform realMoneyPos;
    public Transform happyMoneyPos;
    [SerializeField]
    Vector3 moneyImagePos;

    public void DisplayMoneyCollected(Machine machine)
    {
        if (machine.scheme.moneyCurrency == Currency.RealMoney)
        {
            displayText = displayRealMoneyText;
            moneyImagePos = realMoneyPos.position;// Camera.main.ScreenToWorldPoint(realMoneyPos.position);
        }

        else if (machine.scheme.moneyCurrency == Currency.HappyMoney)
        {
            displayText = displayHappyMoneyText;
            moneyImagePos = happyMoneyPos.position;// Camera.main.ScreenToWorldPoint(happyMoneyPos.position);
        }

        Vector3 startPos = Camera.main.WorldToScreenPoint(machine.transform.position);
        displayText.transform.position = startPos;
       // moneyImagePos = RectTransformUtility.WorldToScreenPoint(null, moneyImagePos);


        if (displayText)
        {
            DisplayMessage("+" + machine.GetReturnedMoney().ToString());
            rectTransform = displayText.GetComponent<RectTransform>();
            rectTransform.gameObject.SetActive(true);

            SetLerping(rectTransform.position, moneyImagePos);
        }
    }

    void SetAlpha()
    {
        if(fadeAlpha != null)
        {
            StopCoroutine(fadeAlpha);
        }

        fadeAlpha = FadeAlpha();
        StartCoroutine(fadeAlpha);

    }

    void SetLerping(Vector3 start, Vector3 target)
    {
        currentTime = 0;
        normalizedValue = 0;
        StartCoroutine(StartLerping(start, target));
    }

    IEnumerator StartLerping(Vector3 start, Vector3 target)
    {
        yield return new WaitForSeconds(displayTime);

        while (currentTime <= timeOfTravel)
        {
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / timeOfTravel; // we normalize our time 

            rectTransform./*anchored*/position = Vector3.Lerp(start, target, normalizedValue);
            yield return null;
        }
        rectTransform.gameObject.SetActive(false);
    }

    IEnumerator FadeAlpha()
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

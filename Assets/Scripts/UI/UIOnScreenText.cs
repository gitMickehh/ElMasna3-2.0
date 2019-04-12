using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOnScreenText : MonoBehaviour
{
    public GameConfig configFile;

    public FloatField realMoney;
    public FloatField happyMoney;

    public Text realMoneyText;
    public Text happyMoneyText;

    [Attributes.GreyOut]
    public float moneyIconSize;

    public Button buildFloorButton;

    private void Start()
    {
        moneyIconSize = realMoneyText.GetComponentInChildren<Image>().rectTransform.rect.width;
        OnRealMoneyChanged();
        OnHappyMoneyChanged();
    }

    public void OnRealMoneyChanged()
    {
        realMoneyText.text = realMoney.GetValue().ToString();

        realMoneyText.GetComponent<RectTransform>().sizeDelta =
            new Vector2((realMoneyText.fontSize * (realMoneyText.text.Length)) + moneyIconSize, realMoneyText.GetComponent<RectTransform>().rect.height);

        if(configFile.FloorCost <= realMoney.GetValue())
        {
            buildFloorButton.interactable = true;
        }
        else
        {
            buildFloorButton.interactable = false;
        }
    }

    public void OnHappyMoneyChanged()
    {
        happyMoneyText.text = happyMoney.GetValue().ToString();

        happyMoneyText.GetComponent<RectTransform>().sizeDelta =
            new Vector2((happyMoneyText.fontSize * (happyMoneyText.text.Length))+ moneyIconSize, happyMoneyText.GetComponent<RectTransform>().rect.height);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOnScreenText : MonoBehaviour
{
    public GameConfig configFile;

    public IntField dayNumber;
    public FloatField realMoney;
    public FloatField happyMoney;

    public Text realMoneyText;
    public Text happyMoneyText;
    public Text dayNumberText;

    [Attributes.GreyOut]
    public float moneyIconSize;

    public Button buildFloorButton;
    public Text buildFloorCost;

    private void Start()
    {
        moneyIconSize = realMoneyText.GetComponentInChildren<Image>().rectTransform.rect.width;
        OnRealMoneyChanged();
        OnHappyMoneyChanged();
        OnUpdateDayNumber();
    }

    public void OnRealMoneyChanged()
    {
        realMoneyText.text = realMoney.GetValue().ToString();

        realMoneyText.GetComponent<RectTransform>().sizeDelta =
            new Vector2((realMoneyText.fontSize * (realMoneyText.text.Length)) + moneyIconSize, realMoneyText.GetComponent<RectTransform>().rect.height);


        if (configFile.FloorCost <= realMoney.GetValue())
        {
            buildFloorButton.interactable = true;
            buildFloorCost.color = Color.white;
        }
        else
        {
            buildFloorButton.interactable = false;
            buildFloorCost.color = Color.grey;
        }

        buildFloorCost.text = configFile.FloorCost.ToString();
    }

    public void OnHappyMoneyChanged()
    {
        happyMoneyText.text = happyMoney.GetValue().ToString();

        happyMoneyText.GetComponent<RectTransform>().sizeDelta =
            new Vector2((happyMoneyText.fontSize * (happyMoneyText.text.Length))+ moneyIconSize, happyMoneyText.GetComponent<RectTransform>().rect.height);
    }

    public void OnUpdateDayNumber()
    {
        dayNumberText.text = dayNumber.GetValue().ToString();
    }
}

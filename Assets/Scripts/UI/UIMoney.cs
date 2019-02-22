using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMoney : MonoBehaviour
{
    public Currencies realMoney;
    public Currencies happyMoney;

    public Text realMoneyText;
    public Text happyMoneyText;

    public void OnRealMoneyChanged()
    {
        realMoneyText.text = realMoney.money.ToString();
    }

    public void OnHappyMoneyChanged()
    {
        happyMoneyText.text = happyMoney.money.ToString();
    }
}

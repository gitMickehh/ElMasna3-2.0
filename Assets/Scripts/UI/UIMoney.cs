using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMoney : MonoBehaviour
{
    public FloatField realMoney;
    public FloatField happyMoney;

    public Text realMoneyText;
    public Text happyMoneyText;

    public void OnRealMoneyChanged()
    {
        realMoneyText.text = realMoney.GetValue().ToString();
    }

    public void OnHappyMoneyChanged()
    {
        happyMoneyText.text = happyMoney.GetValue().ToString();
    }
}

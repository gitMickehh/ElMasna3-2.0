using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICustomize : MonoBehaviour
{
    public GameObject colorPicker;

    void Start()
    {
        colorPicker.SetActive(false);
    }

    public void OnCustomizeButtonClicked()
    {
        if (colorPicker.activeSelf)
        {
            colorPicker.SetActive(false);
        }
        else
        {
            colorPicker.SetActive(true);
        }
    }
}

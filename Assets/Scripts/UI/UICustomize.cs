using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICustomize : MonoBehaviour
{
    public GameObject colorPicker;

    [Header("UI Events")]
    public GameEvent UIOnEvent;
    public GameEvent UIOffEvent;

    void Start()
    {
        colorPicker.SetActive(false);
    }

    public void OnCustomizeButtonClicked()
    {
        if (colorPicker.activeSelf)
        {
            colorPicker.SetActive(false);
            UIOffEvent.Raise();
        }
        else
        {
            colorPicker.SetActive(true);
            UIOnEvent.Raise();
        }
    }
}

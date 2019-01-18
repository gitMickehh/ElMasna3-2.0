using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCycleUI : MonoBehaviour
{
    Slider timerSlider;
    public FloatField timerValue;

    private void Start()
    {
        timerSlider = GetComponent<Slider>();
    }

    private void Update()
    {
        timerSlider.value = timerValue.GetValue();
    }

}

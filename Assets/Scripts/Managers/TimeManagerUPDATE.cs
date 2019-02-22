using Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagerUPDATE : MonoBehaviour
{

    [Header("Timer")]

    [Tooltip("The whole day in game (In Minutes)")]
    public float wholeDayInGame = 60;

    [GreyOut]
    [SerializeField]
    float wholeDayInSeconds;

    [GreyOut]
    [SerializeField]
    float runningTime;

    [Header("Events")]
    public GameEvent StartDay;
    public GameEvent StartNight;
    public GameEvent EndDay;

    [Header("Shared Data")]
    public FloatField dayPercentage;

    private void Start()
    {
        wholeDayInSeconds = wholeDayInGame * 60;
    }

    private void Update()
    {
        runningTime += Time.deltaTime;
        CalculatePercentage();
    }

    private void CalculatePercentage()
    {
        dayPercentage.SetValue((wholeDayInSeconds - runningTime) / wholeDayInSeconds);
    }

    void ResetTimer()
    {
        runningTime = 0;
    }

    public void LoadTimer(float rTime)
    {
        runningTime = rTime;
    }

    public float GetRunningTime()
    {
        return runningTime;
    }
}

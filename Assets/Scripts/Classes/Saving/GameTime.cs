using System;
using System.Collections.Generic;
using UnityEngine;

public enum Day
{
    SUNDAY,
    MONDAY,
    TUESDAY,
    WEDNESDAY,
    THURSDAY,
    FRIDAY,
    SATURDAY
}

[Serializable]
public class GameTime
{
    public float time;
    public Day day;

    public DateClass dTime;

    public GameTime(float t, Day d, DateClass dt)
    {
        time = t;
        day = d;
        dTime = dt;
    }
}

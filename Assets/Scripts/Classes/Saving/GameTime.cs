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

    public DateTime dTime;

    public GameTime(float t, Day d, DateTime dt)
    {
        time = t;
        day = d;
        dTime = dt;
    }
}

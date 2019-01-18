using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DateClass
{

    public int Day;
    public int Month;
    public int Year;

    public int Hour;
    public int Minute;
    public int Second;

    public DateClass(int hour, int min, int sec, int day, int mon, int year)
    {
        Hour = hour;
        Minute = min;
        Second = sec;

        Day = day;
        Month = mon;
        Year = year;
    }

}

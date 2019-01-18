using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Date", menuName = "ElMasna3/Fields/Game Date")]
public class DateTimeScriptable : ScriptableObject {

    public DateClass date;

    //public int Day;
    //public int Month;
    //public int Year;

    //public int Hour;
    //public int Minute;
    //public int Second;

    public void SetTimeNow()
    {
        System.DateTime d = System.DateTime.Now;

        date.Day = d.Day;
        date.Month = d.Month;
        date.Year = d.Year;

        date.Hour = d.Hour;
        date.Minute = d.Minute;
        date.Second = d.Second;
    }

    public static DateClass GetTimeNow()
    {
        System.DateTime d = System.DateTime.Now;
        return new DateClass(d.Hour, d.Minute, d.Second, d.Day, d.Month, d.Year);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Day
{
    Sunday,
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday
}

public class TimeManager : MonoBehaviour {

    [Header("Game Time")]
    public Day dayInGame = Day.Sunday;
    public float dayDurationMinutes = 60f;

    [Header("Time UI")]
    public FloatField timer;

    [Header("Events")]
    public GameEvent startDayEvent;
    public GameEvent endDayEvent;

    [Header("Game Config")]
    public GameConfig GameConfigFile;

    //properties
    public float MorningDuration
    {
        get
        {
            return (dayDurationMinutes * 0.84f);
        }
    }
    public float NightDuration
    {
        get
        {
            return dayDurationMinutes - MorningDuration;
        }
    }

    float timeLeft;
    float timeLeftSec;

    private void Start()
    {
        //here we call a function to calculate t in ContinueTimer(t)
        DateClass timeNow = DateTimeScriptable.GetTimeNow();
        GetSetDay();
        CalculateTimeGap(timeNow);
    }

    private void OnApplicationQuit()
    {
        SaveDay();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveDay();
        }
        else
        {
            DateClass timeNow = DateTimeScriptable.GetTimeNow();
            GetSetDay();
            CalculateTimeGap(timeNow);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            SaveDay();
        }
        else
        {
            DateClass timeNow = DateTimeScriptable.GetTimeNow();
            GetSetDay();
            CalculateTimeGap(timeNow);

            //timer.maxTimeSeconds = dayDurationMinutes * 60;
            //timer.timeLeftSeconds = timeLeft * 60 + timeLeftSec;
        }
    }

    //Calculate Time 
    void CalculateTimeGap(DateClass timeNow)
    {
        //first time difference
        float deltaMinuteFromFirst = GameConfigFile.LastTimePlayed.date.Minute - GameConfigFile.FirstTimeEver.date.Minute;
        if (deltaMinuteFromFirst < 0)
        {
            deltaMinuteFromFirst += (int)dayDurationMinutes;
        }

        float deltaSecondFromFirst = GameConfigFile.LastTimePlayed.date.Second - GameConfigFile.FirstTimeEver.date.Second;
        if (deltaSecondFromFirst < 0)
        {
            deltaSecondFromFirst += 60;
        }
        
        //minutes
        float minuteDifference = timeNow.Minute - GameConfigFile.LastTimePlayed.date.Minute + deltaMinuteFromFirst;
        if (minuteDifference < 0)
        {
            minuteDifference += (int)dayDurationMinutes;
        }
        else if (minuteDifference >= (int)dayDurationMinutes)
        {

        }

        //seconds
        float secondDifference = timeNow.Second - GameConfigFile.LastTimePlayed.date.Second + deltaSecondFromFirst;
        if (secondDifference < 0)
        {
            secondDifference += 60;
        }
        else if (secondDifference >= 60)
        {

        }

        timeLeft = dayDurationMinutes - minuteDifference;
        timeLeftSec = secondDifference % 60;

        //TimeLog.MyLog("Difference from last log in: " + minuteDifference + ":" + secondDifference);
        TimeLog.MyLog("Time left for one hour: " + timeLeft + ":" + timeLeftSec);

        if (timeLeft <= NightDuration)
        {
            //night
            StartCoroutine(ContinueTimerNight(timeLeft*60 + timeLeftSec));
        }
        else
        {
            //morning
            StartCoroutine(ContinueTimer(timeLeft * 60 + timeLeftSec));
        }

        //for score calculation (WAS A FUNCTION) but i merged them into one because of redundant minute and second difference

        //hours
        int hourDifference = timeNow.Hour - GameConfigFile.LastTimePlayed.date.Hour;
        if (hourDifference < 0)
        {
            hourDifference += 24;
        }

        //days
        int dayDifference = timeNow.Day - GameConfigFile.LastTimePlayed.date.Day;
        if (dayDifference < 0)
        {
            dayDifference += 30;
        }

        //Months and years
        int yearsDifference = timeNow.Year - GameConfigFile.LastTimePlayed.date.Year;
        int monthsDifference = timeNow.Month - GameConfigFile.LastTimePlayed.date.Month;
        if (monthsDifference < 0)
        {
            monthsDifference += 12;
        }

        AddDay(yearsDifference, monthsDifference, dayDifference, hourDifference);
        CalculateScore(yearsDifference, monthsDifference, dayDifference, hourDifference, minuteDifference, secondDifference);
    }

    //Calculate Score
    void CalculateScore(int dYear, int dMonth, int dDays, int dHours, float dMinutes, float dSeconds)
    {
        //calculate score here
        float timeInMin = (dYear * (518400)) + (dMonth * (43200)) + (dDays * (1440)) + (dHours * (60)) + dMinutes + (dSeconds / (60.0f));
    }

    //Setting Day
    void AddDay()
    {
        dayInGame++;

        if ((int)dayInGame > 6)
            dayInGame = 0;
    }

    void AddDay(int nOfYears, int nOfMonths, int nOfDays,int nOfHours)
    {
        int totalHours = ConvertToHours(nOfYears,nOfMonths,nOfDays,nOfHours);
        AddDay(totalHours);
    }

    void AddDay(int nOfHours)
    {
        int total = nOfHours + (int)dayInGame;
        dayInGame = (Day)(total % 7);
    }

    //Hour Conversion
    int ConvertToHours(int nOfYears, int nOfMonths, int nOfDays, int nOfHours)
    {
        int totalHours = 0;
        //transform all to hours:
        if (nOfYears == 0)
        {
            if (nOfMonths == 0)
            {
                if (nOfDays == 0)
                {
                    totalHours = nOfHours;
                }
                else
                {
                    totalHours = nOfHours + (nOfDays * 24);
                }
            }
            else
            {
                totalHours = nOfHours + (nOfMonths * 730);
            }
        }
        else
        {
            totalHours = nOfHours + (nOfYears * 8760);
        }

        return totalHours;
    }

    //Player Prefs
    void SaveDay()
    {
        PlayerPrefs.SetInt("Day", (int)dayInGame);
    }

    void GetSetDay()
    {
        var dayN = PlayerPrefs.GetInt("Day");

        dayInGame = (Day)dayN;
    }

    //Coroutines
    IEnumerator ActivateTimer()
    {
        while(true)
        {
            startDayEvent.Raise();
            Debug.Log("Morning, " + dayInGame.ToString() + ", day: " + MorningDuration * 60 + " seconds");
            SetTimerUI(MorningDuration * 60, MorningDuration * 60);
            yield return new WaitForSeconds(MorningDuration * 60);

            endDayEvent.Raise();
            Debug.Log("Night" + ", day: " + (NightDuration) * 60 + " seconds");
            SetTimerUI(NightDuration * 60, NightDuration * 60, 'n');
            yield return new WaitForSeconds(NightDuration * 60);

            AddDay();
        }
    }

    IEnumerator ContinueTimer(float t)
    {
        //called if t (minutes) < MorningDuration
        //t is in seconds
        SetTimerUI(MorningDuration * 60, t);
        yield return new WaitForSeconds(t);

        SetTimerUI(NightDuration * 60, NightDuration * 60,'n');
        yield return new WaitForSeconds(NightDuration * 60);

        AddDay();
        StartCoroutine(ActivateTimer());
    }

    IEnumerator ContinueTimerNight(float t)
    {
        //called if t (minutes) > MorningDuration
        //t is in seconds
        TimeLog.MyLog("Night duration: " + t);
        SetTimerUI(NightDuration * 60,t, 'n');

        yield return new WaitForSeconds(t);

        Debug.Log("Day Ended");
        AddDay();
        StartCoroutine(ActivateTimer());
    }

    void SetTimerUI(float timerMaxTime, float timerCurrentValue, char c = 'd')
    {
        
    }
}

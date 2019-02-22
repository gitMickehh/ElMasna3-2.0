using Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameConfig GameConfigFile;

    [Header("Factory")]
    public FloatField FactoryMoney;
    public FloatField HappyMoney;

    [Header("Building Floors")]
    public GameObject Floor;
    public FloorList listOfFloors;
    public GameEvent BuildSuccess;
    public GameEvent BuildFailure;

    [Header("Time")]
    public Day GameDay;
    [GreyOut]
    [SerializeField]
    TimeManagerUPDATE timer;
    System.DateTime firstTimePlayed;

    private void Start()
    {
        StartCoroutine(lateStart());
    }

    IEnumerator lateStart()
    {
        yield return new WaitForEndOfFrame();

        timer = FindObjectOfType<TimeManagerUPDATE>();
    }

    public void BuildFloor()
    {
        if (FactoryMoney.GetValue() >= GameConfigFile.FloorCost)
        {
            FactoryMoney.AddValue(-GameConfigFile.FloorCost);

            var f = Instantiate(Floor, new Vector3(), new Quaternion());

            float heightOfFloor = f.GetComponentInChildren<Collider>().bounds.max.y;
            Vector3 position = Vector3.up * heightOfFloor * (f.GetComponent<Floor>().floorOrder - 1);
            f.transform.position = position;

            BuildSuccess.Raise();
        }
        else
        {
            BuildFailure.Raise();
        }

    }

    public System.DateTime GetFirstTimePlayed()
    {
        return firstTimePlayed;
    }

    public GameTime GetGameTime()
    {
        return new GameTime(timer.GetRunningTime(), GameDay, System.DateTime.Now);
    }

    public void LoadTime(GameTime gt)
    {
        float LastTimeTimer = gt.time;
        Day savedDay = gt.day;
        System.DateTime lastTime = gt.dTime;
        var timeNow = System.DateTime.Now;

        //days
        int days = 0;
        days += ((Mathf.Abs(lastTime.Year - timeNow.Year) * 12) * 30);
        days += ((Mathf.Abs(lastTime.Month - timeNow.Month) % 12) * 30);
        days += (Mathf.Abs(lastTime.Day - timeNow.Day) % 7);
        GameDay = (Day)((int)savedDay + days);

        //Seconds
        float lastTimeInSeconds = 0;
        float deltaMinutes = Mathf.Abs(lastTime.Minute - timeNow.Minute) % 60;
        lastTimeInSeconds = (deltaMinutes * 60) + lastTime.Second;
        //crazy shit
        float NewTimerValue = Mathf.Abs(lastTimeInSeconds - LastTimeTimer) % 60;

        timer.LoadTimer(NewTimerValue);
    }

}

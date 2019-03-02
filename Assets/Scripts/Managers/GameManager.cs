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
    public FloorList listOfFloors;
    public GameEvent BuildSuccess;
    public GameEvent BuildFailure;

    [Header("Time")]
    [GreyOut]
    public int DayInMonth = 0;
    [GreyOut]
    public int DifferenceInDays = 0;
    [GreyOut]
    [SerializeField]
    TimeManagerUPDATE timer;

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
        if (CheckBalance(GameConfigFile.FloorCost,Currency.RealMoney))
        {
            WithdrawMoney(GameConfigFile.FloorCost,Currency.RealMoney);

            var f = Instantiate(GameConfigFile.FloorPrefab, new Vector3(), new Quaternion());

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

    public bool CheckBalance(float money, Currency currency)
    {
        switch (currency)
        {
            case Currency.RealMoney:
                if (money <= FactoryMoney.GetValue())
                    return true;
                else
                    return false;
            case Currency.HappyMoney:
                if (money <= HappyMoney.GetValue())
                    return true;
                else
                    return false;
        }

        return false;
    }

    public bool WithdrawMoney(float money, Currency currency)
    {
        if (currency == Currency.RealMoney)
        {
            if (money <= FactoryMoney.GetValue())
            {
                FactoryMoney.AddValue(-money);
                return true;
            }
            else
            {
                Debug.LogWarning("Real Money isn't enough!");
                return false;
            }
        }
        else
        {
            if (money <= HappyMoney.GetValue())
            {
                HappyMoney.AddValue(-money);
                return true;
            }
            else
            {
                Debug.LogWarning("Happy Money isn't enough!");
                return false;
            }
        }
    }

    public void DepositMoney(float money, Currency currency)
    {
        if (money < 0)
            return;

        if(currency == Currency.RealMoney)
        {
            FactoryMoney.AddValue(money);
        }
        else
        {
            HappyMoney.AddValue(money);
        }
    }

    public GameTime GetGameTime()
    {

        DateClass dateNow = new DateClass(System.DateTime.Now);
        return new GameTime(timer.GetRunningTime(), timer.GameDay, DayInMonth, dateNow);
    }

    public void LoadTime(GameTime gt)
    {
        float LastTimeTimer = gt.time;
        Day savedDay = gt.day;
        DateClass lastTime = gt.dTime;
        var timeNow = System.DateTime.Now;

        //days
        int days = 0;
        days += ((Mathf.Abs(lastTime.Year - timeNow.Year) * 12) * 30 * 24);
        days += ((Mathf.Abs(lastTime.Month - timeNow.Month) % 12) * 30 * 24);
        days += ((Mathf.Abs(lastTime.Day - timeNow.Day) % 7) * 24);
        days += ((Mathf.Abs(lastTime.Hour - timeNow.Hour) % 24));
        DifferenceInDays = days;

        if (days > 0)
            timer.StartDay.Raise();

        timer.GameDay = (Day)(((int)savedDay + days) % 7);
        //Debug.Log("Difference in days: " + DifferenceInDays + ", It was " + savedDay +
        //    "\n Today: " + GameDay);

        //Debug.Log("Last timer: " + LastTimeTimer);

        var deltaMinute = Mathf.Abs(lastTime.Minute - timeNow.Minute) * 60;
        var deltaSeconds = Mathf.Abs(lastTime.Second - timeNow.Second) + deltaMinute;

        //Debug.Log("time difference: " + deltaSeconds + " seconds.");

        //Debug.Log("Time now:\n" + timeNow);
        //Debug.Log("last time:\n" + lastTime);

        timer.LoadTimer((deltaSeconds + LastTimeTimer) % timer.GetWholeDayInSeconds());
    }

}

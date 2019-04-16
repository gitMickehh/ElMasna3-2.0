using Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public GameConfig GameConfigFile;
    public GameObjectField gameManagerField;

    [Header("Factory")]
    public FloatField FactoryMoney;
    public FloatField HappyMoney;

    [Header("Building Floors")]
    public FloorList listOfFloors;
    public GameEvent BuildSuccess;
    public GameEvent BuildFailure;

    [Header("Live Objects")]
    public WorkerField SelectedWorker;
    public FloatField machineCheck;
    public MachineList machineList;

    [Header("Time")]
    [GreyOut]
    public int DayInMonth = 0;
    [GreyOut]
    public int DifferenceInDays = 0;
    [GreyOut]
    [SerializeField]
    TimeManagerUPDATE timer;

    [Header("Events")]
    public GameEvent closeUIPanel;
    public GameEvent workerIsHired;

    //worker mods
    private float timerOfWorkersHappiness = 0;

    //Modal Panel
    private ModalPanel modalPanel;
    private UnityAction BuildFloorAction;

    public WorkerList workerList;

    [Header("Save Game Events")]
    public float timeBetweenSaves = 60f;
    private float timerToSavegame;
    public GameEvent SaveGameEvent;

    private void OnEnable()
    {
        gameManagerField.gameObjectReference = gameObject;
        
    }

    private void OnDisable()
    {
        gameManagerField.gameObjectReference = null;
    }

    private void Start()
    {
        StartCoroutine(lateStart());
        modalPanel = ModalPanel.Instance();
        timerToSavegame = 0;

        BuildFloorAction = new UnityAction(ConfirmBuildFloor);
    }

    private void Update()
    {
        if(timerToSavegame >= timeBetweenSaves)
        {
            timerToSavegame = 0;
            SaveGameEvent.Raise();
        }
        else
        {
            timerToSavegame += Time.deltaTime;
        }

        if (timerOfWorkersHappiness >= GameConfigFile.timeToDecreaseHappiness)
        {
            workerList.UpdateWorkers();
        }
        else
        {
            timerOfWorkersHappiness += Time.deltaTime;
        }
    }

    IEnumerator lateStart()
    {
        yield return new WaitForEndOfFrame();

        timer = FindObjectOfType<TimeManagerUPDATE>();
    }

    public void SaveGameTimerReset()
    {
        timerToSavegame = 0;
    }

    public void BuildFloorButton()
    {
        var lang = GameConfigFile.CurrentLanguageProfile;
        string q = lang.GetQuestion(lang.DoYouWantToBuildANewFloor);

        string[] s = new string[] {
            q,
            lang.YouWillPay,
            GameConfigFile.FloorCost.ToString()
        };

        modalPanel.Choice(lang.GetStatement(s), BuildFloorAction);
    }

    private void ConfirmBuildFloor()
    {
        if (CheckBalance(GameConfigFile.FloorCost, Currency.RealMoney))
        {
            WithdrawMoney(GameConfigFile.FloorCost, Currency.RealMoney);

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

    public bool WithdrawMoney(float money, Currency currency, bool soundOn = true)
    {
        if (currency == Currency.RealMoney)
        {
            if (money <= FactoryMoney.GetValue())
            {
                if(soundOn)
                    AudioManager.instance.Play("Ka-Ching");

                FactoryMoney.AddValue(-money);
                return true;
            }
            else
            {
                Debug.LogWarning("Real Money isn't enough!");
                return false;
            }
        }
        else if (currency == Currency.HappyMoney)
        {
            if (money <= HappyMoney.GetValue())
            {
                if(soundOn)
                    AudioManager.instance.Play("Ka-Ching");

                HappyMoney.AddValue(-money);
                return true;
            }
            else
            {
                Debug.LogWarning("Happy Money isn't enough!");
                return false;
            }
        }

        return false;
    }

    public void DepositMoney(float money, Currency currency, bool soundOn = true)
    {
        if (money < 0)
            return;

        if (currency == Currency.RealMoney)
        {
            FactoryMoney.AddValue(money);
        }
        else
        {
            HappyMoney.AddValue(money);
        }

        if(soundOn)
            AudioManager.instance.Play("MoneyIn");
    }

    public void StartNewDay()
    {
        timer.StartDay.Raise();
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

        //we do not count in Delta Hours (Hour means day in game) because an hour is a full cycle
        //it is not very dynamic if we want to change that in the game, but for now it does the deed

        var deltaMinute = Mathf.Abs(lastTime.Minute - timeNow.Minute) * 60;
        var deltaSeconds = Mathf.Abs(lastTime.Second - timeNow.Second) + deltaMinute;

        var previousTimeInSeconds = deltaSeconds + LastTimeTimer;
        
        timer.LoadTimer(previousTimeInSeconds % timer.GetWholeDayInSeconds());

        //a day is one hour (60 minutes)
        var totalTimePassed = previousTimeInSeconds + days * 60;
        //AddMoneyOfOfflineTime(totalTimePassed);
        CalculateTimeOfMachines(totalTimePassed);
    }
    private void CalculateTimeOfMachines(float t)
    {
        machineList.CalculateMachinesTimePassed(t);
    }

    private void AddMoneyOfOfflineTime(float t)
    {
        var rMoney = machineList.GetPastCyclesMoneyByCurrency(t, Currency.RealMoney);
        DepositMoney(rMoney,Currency.RealMoney,false);

        var hMoney = machineList.GetPastCyclesMoneyByCurrency(t, Currency.HappyMoney);
        DepositMoney(hMoney, Currency.HappyMoney, false);
    }

    public void HireConfirmation()
    {
        Debug.Log("Hire Confirmation");

        if (SelectedWorker.worker == null)
        {
            Debug.LogWarning("Selected worker is null.");
            return;
        }

        LanguageProfile lang = GameConfigFile.CurrentLanguageProfile;
        string[] qs = new string[] {
            lang.HireWorker,
            SelectedWorker.worker.FullName,
        };

        string[] statement = new string[] {
            "\n",
            lang.YouWillPay,
            GameConfigFile.HiringCost.ToString("0")
        };

        string message = lang.GetQuestion(qs);
        message += lang.GetStatement(statement);

        modalPanel.Choice(message, new UnityAction(HireWorker), GameConfigFile.icons[0]);
    }

    public void HireWorker()
    {
        if (!CheckBalance(GameConfigFile.HiringCost, Currency.RealMoney))
        {
            modalPanel.Message(GameConfigFile.CurrentLanguageProfile.NotEnoughMoney,GameConfigFile.icons[0]);
            return;
        }

        if (!listOfFloors.CheckAvailability())
        {
            //no place for worker
            Debug.LogWarning("No room for the worker");
            return;
        }


        var worker = SelectedWorker.worker;
        worker.inOrientation = false;

        var machine = listOfFloors.GetFirstAvailableMachine();

        if (machine == null)
        {
            var emptyWaypoint = listOfFloors.GetFirstAvailableBreakSpace();

            if (emptyWaypoint == null)
            {
                //no place for worker
                Debug.LogWarning("No room for the worker!\n[CheckAvailability() in list of floors didn't work]");
                return;
            }

            //send worker to empty break room
            worker.gameObject.GetComponent<SeekRoom>().SwitchRoom(emptyWaypoint, emptyWaypoint.GetComponentInParent<Floor>().WorkersHolder);
            //worker.transform.SetParent(emptyWaypoint.GetComponentInParent<Floor>().WorkersHolder);
            var bR = emptyWaypoint.GetComponentInParent<Floor>().GetBreakRoomPlace(emptyWaypoint);
            bR.worker = worker;
        }
        else
        {
            //send worker to empty machine
            machine.SetWorker(worker);
            //worker.transform.SetParent(machine.parentFloor.WorkersHolder);
            WayPoint wayPointTarget = machine.gameObject.GetComponentInParent<WayPoint>();
            worker.gameObject.GetComponent<SeekRoom>().SwitchRoom(wayPointTarget, machine.parentFloor.WorkersHolder);
        }
        workerIsHired.Raise();
        WithdrawMoney(GameConfigFile.HiringCost, Currency.RealMoney);
        closeUIPanel.Raise();
        SaveGameEvent.Raise();
    }

    public void PartyHandling()
    {
        workerList.AddPartyHappiness(GameConfigFile.happinessPercentage);
        WithdrawMoney(GameConfigFile.PartyCost, Currency.HappyMoney);
    }

    public void PayForMachine()
    {
        WithdrawMoney(machineCheck.GetValue(), Currency.RealMoney);
        machineCheck.SetValue(0);
    }

    public void LoadFactoryData(FactoryData data)
    {
        FactoryMoney.SetValue(data.realMoney);
        HappyMoney.SetValue(data.happyMoney);
        GameConfigFile.FloorCost = data.floorBuildCost;
        GameConfigFile.PartyCost = data.partyCost;
        GameConfigFile.SetColorField(data.uniformColor);
        GameConfigFile.timeToDecreaseHappiness = data.timeOfHappinessDecrease;
    }

    public FactoryData GetSaveData()
    {
        return new FactoryData(FactoryMoney.GetValue(), HappyMoney.GetValue(), GameConfigFile.PartyCost, GameConfigFile.HiringCost, GameConfigFile.FloorCost);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public enum MachineState
//{
//    IDLE,
//    WORKING,
//    ONHOLD
//}

public class Machine : MonoBehaviour
{
    [Attributes.GreyOut]
    public Floor parentFloor;
    public GameObjectField gameManagerObject;
    public GameObject machineDoneObject;

    [Header("Machine")]
    public int machineID;
    public int machineModelID;
    public MachineList listOfMahcines;
    [Attributes.GreyOut]
    public bool IsWorking;
    private bool isWaiting;
    //public MachineState machineState;

    [Header("Worker")]
    public GameObject CurrentWorker;
    public Transform workerPosition;

    private DisplayManager displayManager;

    //Machine Work
    [Header("Machine Scheme")]
    public MachineScheme scheme;

    [Header("Timing")]
    public Slider machineTimeSlider;
    public Image machineCircularTimer;
    public bool circular;
    private float timeOfCycle;
    private float runningTime = 0;
    [SerializeField]
    private Animator machineAnimator;
    private Animator machineUIAnimation;

    public int CurrentWorkerID
    {
        get
        {
            if (CurrentWorker != null)
                return CurrentWorker.GetComponent<Worker>().ID;
            else
                return 0;
        }
    }

    private void Awake()
    {
        displayManager = DisplayManager.Instance();
    }
    private void OnEnable()
    {
        machineID = listOfMahcines.GetNewId();
        listOfMahcines.Add(this);
        transform.name = "Machine " + machineID;

        timeOfCycle = scheme.timeOfCycle;

        machineAnimator = GetComponentInChildren<Animator>();
        machineUIAnimation = GetComponentInChildren<LookAtCamera>().GetComponent<Animator>();

        if (circular)
            machineCircularTimer.fillAmount = 0;
        else
            machineTimeSlider.value = 0;

        SliderToggle();
    }

    private void OnDisable()
    {
        listOfMahcines.Remove(this);
    }

    private void Update()
    {
        if (IsWorking && !isWaiting)
        {
            if (runningTime >= timeOfCycle)
            {
                FinishCycle();
            }
            else
            {
                runningTime += Time.deltaTime;
                if (circular)
                    machineCircularTimer.fillAmount = (runningTime / timeOfCycle);
                else
                    machineTimeSlider.value = (runningTime / timeOfCycle);
            }
        }
    }

    /// <summary>
    /// It brings the amount of money it gained during the time off of the game, the input is in seconds. It also sets the time of the machine
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public float GetMoneyMissedAndSetNew(float time)
    {
        float result = 0;

        float cyclesFraction = time / scheme.timeOfCycle;
        int numberOfCycles = Mathf.FloorToInt(cyclesFraction);

        //setting remaining time
        float remainingTime = (cyclesFraction - numberOfCycles) * scheme.timeOfCycle;
        remainingTime += runningTime;

        if (remainingTime / scheme.timeOfCycle >= 1)
        {
            //give money of the new cycle also
            numberOfCycles += (Mathf.FloorToInt(remainingTime / scheme.timeOfCycle));
            remainingTime -= (Mathf.FloorToInt(remainingTime / scheme.timeOfCycle) * scheme.timeOfCycle);
        }

        runningTime = remainingTime;

        result = numberOfCycles * scheme.moneyInCycle;
        return result;
    }

    public SerializableMachine GetSerializableMachine()
    {
        SerializableMachine sm;

        if (CurrentWorker == null)
            sm = new SerializableMachine(true, machineID, machineModelID);
        else
            sm = new SerializableMachine(true, machineID, machineModelID, CurrentWorker.GetComponent<Worker>().ID, runningTime);

        return sm;
    }

    public void LoadMachine(SerializableMachine machineSaved)
    {
        IsWorking = false;
        runningTime = machineSaved.runningTime;
        machineID = machineSaved.machineID;
        SliderToggle();
    }

    public void LoadMachine(SerializableMachine machineSaved, Worker w)
    {
        IsWorking = true;
        runningTime = machineSaved.runningTime;
        machineID = machineSaved.machineID;
        CurrentWorker = w.gameObject;

        SliderToggle();
    }

    public void SetTimer(float timerValue)
    {
        runningTime = timerValue;
        StartCycle();
    }

    private void FinishCycle()
    {
        isWaiting = true;
        machineDoneObject.SetActive(true);
        machineUIAnimation.SetTrigger("Done");

        if (circular)
            machineCircularTimer.fillAmount = 1;
        else
            machineTimeSlider.value = 1;

        IsWorking = false;
        SliderToggle();

        //machineState = MachineState.ONHOLD;
    }

    private void GatherMoney()
    {
        ReturnMoney();
        runningTime = 0;

        if (CurrentWorker != null)
            IsWorking = true;

        displayManager.DisplayMoneyCollected(this);
        SliderToggle();
    }

    private void StartCycle()
    {
        machineDoneObject.SetActive(false);
        machineUIAnimation.SetTrigger("Done");
        runningTime = 0;
        isWaiting = false;
        //machineState = MachineState.WORKING;
    }

    public void ClickMachine()
    {
        if(isWaiting)
        {
            GatherMoney();
            StartCycle();
        }
        else
        {
            //show machine details
            Debug.Log("machine details");
        }
    }

    public void FinishCycleNow()
    {
        FinishCycle();
    }

    public void ChangeWorker(Worker w)
    {
        if (w.currentMachine != null)
        {
            if (CurrentWorker != null)
            {
                //switching workers
                Debug.Log("Switching WORKERS");

                var otherMachine = w.currentMachine;

                otherMachine.CurrentWorker = CurrentWorker;
                CurrentWorker.GetComponent<Worker>().currentMachine = otherMachine;
                //CurrentWorker.GetComponent<SeekRoom>().SwitchRoom(otherMachine.GetComponentInParent<WayPoint>());


                if (parentFloor.floorOrder != otherMachine.parentFloor.floorOrder)
                {
                    //w.transform.SetParent(parentFloor.WorkersHolder);
                    //CurrentWorker.transform.SetParent(otherMachine.parentFloor.WorkersHolder);
                    CurrentWorker.GetComponent<SeekRoom>().SwitchRoom(otherMachine.GetComponentInParent<WayPoint>(), otherMachine.parentFloor.WorkersHolder);

                }
                else
                {
                    CurrentWorker.GetComponent<SeekRoom>().SwitchRoom(otherMachine.GetComponentInParent<WayPoint>());
                }
            }
            else
            {
                w.currentMachine.IsWorking = false;
                w.currentMachine.CurrentWorker = null;
                w.currentMachine.SliderToggle();
            }
        }

        w.currentMachine = this;
        CurrentWorker = w.gameObject;


        CurrentWorker.GetComponent<Worker>().SetWorkerState(WorkerState.Working);
        WayPoint wayPointTarget = gameObject.GetComponentInParent<WayPoint>();
        w.gameObject.GetComponent<SeekRoom>().SwitchRoom(wayPointTarget, parentFloor.WorkersHolder);

        IsWorking = true;
        SliderToggle();
    }

    public void SetWorker(Worker w)
    {
        CurrentWorker = w.gameObject;
        w.currentMachine = this;
        w.SetWorking(true);

        //CurrentWorker.transform.SetParent(parentFloor.WorkersHolder);
        //WayPoint wayPointTarget = gameObject.GetComponentInParent<WayPoint>();
        //w.gameObject.GetComponent<SeekRoom>().SwitchRoom(wayPointTarget);

        IsWorking = true;
        SliderToggle();
    }

    public void SetUpMachine(Worker w, float timerValue)
    {
        CurrentWorker = w.gameObject;
        IsWorking = true;
        SliderToggle();
        runningTime = timerValue;
    }

    private void ReturnMoney()
    {
        var gm = gameManagerObject.gameObjectReference.GetComponent<GameManager>();

        float money = GetReturnedMoney();
        

        gm.DepositMoney(money, scheme.moneyCurrency);
    }

    public float GetReturnedMoney()
    {
        float money = scheme.moneyInCycle;
        var worker = CurrentWorker.GetComponent<Worker>();
        money = Mathf.RoundToInt((worker.happyMeter / 100) * money);
        money = Mathf.Clamp(money, scheme.minimumMoneyGain, scheme.moneyInCycle);
        return money;
    }

    public void CalculateMissingTime(float t)
    {
        float cyclesFraction = t / scheme.timeOfCycle;
        int numberOfCycles = Mathf.FloorToInt(cyclesFraction);

        if(numberOfCycles >= 1)
        {
            FinishCycle();
            return;
        }

        //setting remaining time
        float remainingTime = (cyclesFraction - numberOfCycles) * scheme.timeOfCycle;
        remainingTime += runningTime;

        if (remainingTime / scheme.timeOfCycle >= 1)
        {
            //give money of the new cycle also
            numberOfCycles += (Mathf.FloorToInt(remainingTime / scheme.timeOfCycle));
            remainingTime -= (Mathf.FloorToInt(remainingTime / scheme.timeOfCycle) * scheme.timeOfCycle);
        }

        runningTime = remainingTime;
    }

    public void SliderToggle()
    {
        if(IsWorking)
        {
            if (circular)
                machineCircularTimer.gameObject.SetActive(IsWorking);
            else
                machineTimeSlider.gameObject.SetActive(IsWorking);
        }

        machineAnimator.SetBool("IsWorking", IsWorking);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (workerPosition != null)
            Gizmos.DrawWireSphere(workerPosition.position, 0.5f);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Machine : MonoBehaviour
{
    [Attributes.GreyOut]
    public Floor parentFloor;
    public GameObjectField gameManagerObject;

    [Header("Machine")]
    public int machineID;
    public int machineModelID;
    public MachineList listOfMahcines;
    [Attributes.GreyOut]
    public bool IsWorking;

    [Header("Worker")]
    public GameObject CurrentWorker;
    public Transform workerPosition;

    //Machine Work
    [Header("Machine Scheme")]
    public MachineScheme scheme;
    public Slider machineTimeSlider;
    public Image machineCircularTimer;
    public bool circular;
    private float timeOfCycle;
    private float runningTime = 0;
    [SerializeField]
    private Animator machineAnimator;

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

    private void OnEnable()
    {
        machineID = listOfMahcines.GetNewId();
        listOfMahcines.Add(this);
        transform.name = "Machine " + machineID;

        timeOfCycle = scheme.timeOfCycle;

        machineAnimator = GetComponentInChildren<Animator>();
        SliderToggle();
    }

    private void OnDisable()
    {
        listOfMahcines.Remove(this);
    }

    private void Update()
    {
        if (IsWorking)
        {
            if (runningTime >= timeOfCycle)
            {
                runningTime = 0;
                var gm = gameManagerObject.gameObjectReference.GetComponent<GameManager>();
                gm.DepositMoney(scheme.moneyInCycle, scheme.moneyCurrency);
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
    /// It brings the amount of money it gained during the time off of the game, the input is in seconds.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public float GetMoneyMissed(float time)
    {
        float result = 0;
        float totalTime = scheme.timeOfCycle;


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
    }

    public void FinishCycleNow()
    {
        runningTime = 0;
        var gm = gameManagerObject.gameObjectReference.GetComponent<GameManager>();
        gm.DepositMoney(scheme.moneyInCycle, scheme.moneyCurrency);
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
        w.gameObject.GetComponent<SeekRoom>().SwitchRoom(wayPointTarget,parentFloor.WorkersHolder);

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

    public void SliderToggle()
    {
        if (circular)
            machineCircularTimer.gameObject.SetActive(IsWorking);
        else
            machineTimeSlider.gameObject.SetActive(IsWorking);
        machineAnimator.SetBool("IsWorking", IsWorking);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (workerPosition != null)
            Gizmos.DrawWireSphere(workerPosition.position, 0.5f);
    }

}

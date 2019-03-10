using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Machine : MonoBehaviour
{
    [Attributes.GreyOut]
    public Floor parentFloor;

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
    private float timeOfCycle;
    private float moneyInCycle;
    private Currency moneyCurrency;
    private float runningTime = 0;

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
        moneyInCycle = scheme.moneyInCycle;
        moneyCurrency = scheme.moneyCurrency;
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

            }
            else
            {
                runningTime += Time.deltaTime;
                machineTimeSlider.value = 1 - (runningTime / timeOfCycle);
            }

        }
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

    public void ChangeWorker(Worker w)
    {
        Debug.Log(gameObject.name + " changed worker");

        if (w.currentMachine != null)
        {
            if (CurrentWorker != null)
            {
                w.currentMachine.CurrentWorker = CurrentWorker;
            }
            else
            {
                w.currentMachine.CurrentWorker = null;
                w.currentMachine.IsWorking = false;
                w.currentMachine.SliderToggle();
            }
        }

        if (parentFloor.floorOrder == w.currentMachine.parentFloor.floorOrder)
        {
            w.transform.SetParent(parentFloor.WorkersHolder);
        }

        w.currentMachine = this;
        CurrentWorker = w.gameObject;

        WayPoint wayPointTarget = gameObject.GetComponentInParent<WayPoint>();
        w.gameObject.GetComponent<SeekRoom>().SwitchRoom(wayPointTarget);

        IsWorking = true;
        w.currentMachine.SliderToggle();
    }

    public void SetWorker(Worker w)
    {
        CurrentWorker = w.gameObject;
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
        machineTimeSlider.gameObject.SetActive(IsWorking);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (workerPosition != null)
            Gizmos.DrawWireSphere(workerPosition.position, 0.5f);
    }

}

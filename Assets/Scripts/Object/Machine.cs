using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }

    public void LoadMachine(SerializableMachine machineSaved, Worker w)
    {

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
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (workerPosition != null)
            Gizmos.DrawWireSphere(workerPosition.position, 0.5f);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    public int machineID;
    public int machineModelID;
    public MachineList listOfMahcines;

    [Header("Worker")]
    public GameObject CurrentWorker;
    public Transform workerPosition;

    public int CurrentWorkerID
    {
        get {
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
    }

    private void OnDisable()
    {
        listOfMahcines.Remove(this);
    }

    public SerializableMachine GetSerializableMachine()
    {
        SerializableMachine sm;

        if (CurrentWorker == null)
            sm = new SerializableMachine(true,machineID, machineModelID);
        else
            sm = new SerializableMachine(true,machineID, machineModelID, CurrentWorker.GetComponent<Worker>().ID);

        return sm;
    }

    public void ChangeWorker(Worker w)
    {
        CurrentWorker = w.gameObject;

        w.gameObject.transform.position = workerPosition.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (workerPosition != null)
            Gizmos.DrawWireSphere(workerPosition.position, 0.5f);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    public int machineID;

    [Header("Worker")]
    public GameObject CurrentWorker;
    public Transform workerPosition;

    public SerializableMachine GetSerializableMachine()
    {
        SerializableMachine sm;

        if (CurrentWorker == null)
            sm = new SerializableMachine(true,machineID);
        else
            sm = new SerializableMachine(true,machineID,CurrentWorker.GetComponent<Worker>().ID);

        return sm;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (workerPosition != null)
            Gizmos.DrawWireSphere(workerPosition.position, 0.5f);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public class UIRoom
{
    public UIMachine[] Machines;
}

public class UIFloor : MonoBehaviour
{
    public Transform WorkerMapSpawn;
    public GameObject WorkerImage;
    public WorkerList listOfWorkers;
    public Floor realFloor;

    [Header("Rooms")]
    public UIRoom[] rooms;

    public void UpdateFloor()
    {
        for (int i = 0; i < realFloor.workRooms.Length; i++)
        {
            var m = realFloor.workRooms;

            for (int j = 0; j < m[i].machinePlaces.Length; j++)
            {
                if (m[i].machinePlaces[j].machine != null)
                {
                    rooms[i].Machines[j].machineReference = m[i].machinePlaces[j].machine;

                    if (m[i].machinePlaces[j].machine.CurrentWorkerID != 0)
                    {
                        if (rooms[i].Machines[j].workerImage == null)
                        {
                            var wImage = Instantiate(WorkerImage, WorkerMapSpawn);

                            wImage.GetComponent<WorkerUIIcon>().workerID = m[i].machinePlaces[j].machine.CurrentWorkerID;
                            rooms[i].Machines[j].workerImage = wImage;
                            wImage.GetComponent<WorkerUIIcon>().machine = rooms[i].Machines[j];

                            wImage.transform.position = rooms[i].Machines[j].gameObject.transform.position;

                        }
                    }
                }
                else
                {
                    rooms[i].Machines[j].gameObject.SetActive(false);
                }

            }
        }

        transform.name = realFloor.name + " UI";
    }

}

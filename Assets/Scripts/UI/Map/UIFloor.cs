using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public class UIRoom
{
    public UIMapDraggingContainer[] Machines;
}

public class UIFloor : MonoBehaviour
{
    [Header("UI Elements")]
    public Transform WorkerMapSpawn;
    public WorkerList listOfWorkers;
    public GameObject WorkerImage;

    private List<WorkerUIIcon> workers = new List<WorkerUIIcon>();

    [Header("Data")]
    [Attributes.GreyOut]
    public Floor realFloor;
    public PrefabsList machinesPrefabsList;
    public PrefabsList workersPrefabsList;

    [Header("Rooms")]
    public UIRoom[] rooms;
    public UIRoom breakRoom;

    private void Start()
    {
        for (int i = 0; i < realFloor.breakRoom.Length; i++)
        {
            breakRoom.Machines[i].waypoint = realFloor.breakRoom[i].position;
        }
    }

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
                    rooms[i].Machines[j].GetComponent<Image>().sprite = machinesPrefabsList.GetSpriteByID(m[i].machinePlaces[j].machine.machineModelID);

                    if (m[i].machinePlaces[j].machine.CurrentWorkerID != 0)
                    {
                        if (rooms[i].Machines[j].workerImage == null)
                        {
                            var wImage = Instantiate(WorkerImage, WorkerMapSpawn);

                            wImage.GetComponent<WorkerUIIcon>().workerID = m[i].machinePlaces[j].machine.CurrentWorkerID;
                            rooms[i].Machines[j].workerImage = wImage;
                            wImage.GetComponent<WorkerUIIcon>().machine = rooms[i].Machines[j];
                            wImage.transform.position = rooms[i].Machines[j].gameObject.transform.position;

                            workers.Add(wImage.GetComponent<WorkerUIIcon>());
                        }
                    }
                }
                else
                {
                    rooms[i].Machines[j].gameObject.SetActive(false);
                }

            }
        }

        for (int i = 0; i < realFloor.breakRoom.Length; i++)
        {
            //breakRoom.Machines[i].workerImage;
        }

        transform.name = realFloor.name + " UI";
    }


}

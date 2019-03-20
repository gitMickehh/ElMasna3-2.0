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

    private List<WorkerUIIcon> workersInMyFloor = new List<WorkerUIIcon>();

    [Header("Data")]
    [Attributes.GreyOut]
    public Floor realFloor;
    public PrefabsList machinesPrefabsList;
    public WorkerUIIconList workerUIIconsList;

    [Header("Rooms")]
    public UIRoom[] rooms;
    public UIRoom breakRoom;

    private void Start()
    {
        WayPointsAssignment();

        for (int i = 0; i < realFloor.workRooms.Length; i++)
        {

        }
    }

    private void WayPointsAssignment()
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
                    int workerId = m[i].machinePlaces[j].machine.CurrentWorkerID;

                    if (workerId != 0)
                    {
                        WorkerUIIcon NewWorkerIcon = workerUIIconsList.GetWorkerIconByID(workerId);

                        if (NewWorkerIcon == null)
                        {
                            Debug.Log("Instantating with ID " + workerId);
                            NewWorkerIcon = Instantiate(WorkerImage, WorkerMapSpawn).GetComponent<WorkerUIIcon>();
                            NewWorkerIcon.workerID = workerId;
                            NewWorkerIcon.transform.name = "UI" + m[i].machinePlaces[j].machine.CurrentWorker.GetComponent<Worker>().FullName;
                        }

                        NewWorkerIcon.transform.SetParent(rooms[i].Machines[j].gameObject.transform);
                        NewWorkerIcon.transform.localPosition = new Vector2();
                        //workersInMyFloor.Add(wImage.GetComponent<WorkerUIIcon>());
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
            if(realFloor.breakRoom[i].worker != null)
            {
                //break room waypoint has a worker
                int wID = realFloor.breakRoom[i].worker.ID;
                WorkerUIIcon NewWorkerIcon = workerUIIconsList.GetWorkerIconByID(wID);

                if (NewWorkerIcon == null)
                {
                    NewWorkerIcon = Instantiate(WorkerImage, WorkerMapSpawn).GetComponent<WorkerUIIcon>();
                    NewWorkerIcon.workerID = wID;
                    NewWorkerIcon.transform.name = "UI" + realFloor.breakRoom[i].worker.FullName;
                }

                //Debug.Log("break room " + i + ": "+ breakRoom.Machines[i].transform.localPosition);
                //NewWorkerIcon.transform.localPosition = breakRoom.Machines[i].transform.localPosition;
                NewWorkerIcon.transform.SetParent(breakRoom.Machines[i].transform);
                NewWorkerIcon.transform.localPosition = new Vector2();
                breakRoom.Machines[i].workerImage = NewWorkerIcon.gameObject;
            }
        }

        transform.name = realFloor.name + " UI";
    }


}

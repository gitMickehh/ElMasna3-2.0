using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SaveGameManager : MonoBehaviour
{
    [Header("Lists")]
    public FloorList listOfFloors;
    public WorkerList listOfWorkers;

    private Floor firstFloor;

    [Header("Prefabs")]
    public GameObject FloorPrefab;
    public PrefabsList WorkersPrefabs;

    private void Start()
    {
        LoadWorkers();
        LoadFloors();
    }

    private void SaveFloors()
    {
        listOfFloors.SortFloorList();

        for (int i = 0; i < listOfFloors.Items.Count; i++)
        {
            listOfFloors.Items[i].FillSerializableFloor();

            string floorJson = JsonUtility.ToJson(listOfFloors.Items[i].savingFloor);
            Debug.Log(floorJson);

            PlayerPrefs.SetString("floor" + i, floorJson);
        }

        PlayerPrefs.SetInt("floorCount", listOfFloors.Items.Count);
    }

    private void LoadFloors()
    {
        if (PlayerPrefs.HasKey("floorCount"))
        {
            //because first floor is always in the scene.
            firstFloor = listOfFloors.Items[0];
            firstFloor.savingFloor =
                JsonUtility.FromJson<SerializableFloor>(PlayerPrefs.GetString("floor0"));
            firstFloor.LoadFloor();

            var heightOfFloor = firstFloor.GetComponentInChildren<Collider>().bounds.max.y;
            
            //need to intialize the other floors
            int floorCount = PlayerPrefs.GetInt("floorCount");
            for (int i = 1; i < floorCount; i++)
            {
                string retrievedJson = PlayerPrefs.GetString("floor" + i);

                //instantiate floor
                Vector3 position = Vector3.up * heightOfFloor * i;
                GameObject floor = Instantiate(FloorPrefab, position, new Quaternion());

                floor.GetComponent<Floor>().savingFloor =
                    JsonUtility.FromJson<SerializableFloor>(retrievedJson);
                floor.GetComponent<Floor>().LoadFloor();
            }
        }
        else
        {
            Debug.Log("floor count is not saved");
        }

    }

    private void SaveWorkers()
    {
        for (int i = 0; i < listOfWorkers.Items.Count; i++)
        {
            string workerJson = JsonUtility.ToJson(listOfWorkers.Items[i].GetWorkerData());
            Debug.Log(workerJson);

            PlayerPrefs.SetString("worker"+i,workerJson);
        }

        PlayerPrefs.SetInt("workersCount", listOfWorkers.Items.Count);
    }

    private void LoadWorkers()
    {
        if (PlayerPrefs.HasKey("workersCount"))
        {
            int workersCount = PlayerPrefs.GetInt("workersCount");

            for (int i = 0; i < workersCount; i++)
            {
                string retrievedJson = PlayerPrefs.GetString("worker" + i);
                var workerData =  JsonUtility.FromJson<SerializableWorker>(retrievedJson);
                GameObject workerPrefab = WorkersPrefabs.GetPrefabByID(workerData.modelID);
                
                GameObject worker = Instantiate(workerPrefab, new Vector3(), new Quaternion());
                worker.GetComponent<Worker>().LoadWorkerData(workerData);
            }
        }
        else
        {
            Debug.LogWarning("workersCount is not saved!");
        }
    }

    private void SaveAll()
    {
        SaveFloors();
        SaveWorkers();
    }

    private void OnApplicationQuit()
    {
        SaveAll();
    }

    public void ClearSave()
    {
        Debug.Log("Cleared all saves!");
        PlayerPrefs.DeleteAll();
    }

}

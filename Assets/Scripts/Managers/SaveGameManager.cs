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

    [Header("Game Config File")]
    public GameConfig GameConfigFile;

    OrientationBuilding orientation;
    GameManager gManager;

#if UNITY_EDITOR
    [Header("Editor Options")]
    public bool save;
    public bool load;
#endif

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        orientation = FindObjectOfType<OrientationBuilding>();

        //because the game manager finds the timer at the end of the first frame
        yield return new WaitForEndOfFrame();
        gManager = FindObjectOfType<GameManager>();

#if UNITY_EDITOR
        if (load)
        {
#endif
            LoadAll();
#if UNITY_EDITOR
        }
#endif
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
                GameObject floor = Instantiate(GameConfigFile.FloorPrefab, position, new Quaternion());

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

            PlayerPrefs.SetString("worker" + i, workerJson);
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
                var workerData = JsonUtility.FromJson<SerializableWorker>(retrievedJson);
                GameObject workerPrefab = GameConfigFile.WorkersPrefabs.GetPrefabByID(workerData.modelID);

                GameObject worker = Instantiate(workerPrefab, new Vector3(), new Quaternion());

                if (workerData.inOrientation)
                {
                    for (int j = 0; j < orientation.WorkersPositions.Length; j++)
                    {
                        if (orientation.WorkersPositions[j].worker == null)
                        {
                            worker.transform.SetParent(orientation.WorkersPositions[j].position);
                            worker.transform.localPosition = new Vector3();
                            worker.transform.localRotation = new Quaternion();
                            orientation.WorkersPositions[j].worker = worker.GetComponent<Worker>();
                            break;
                        }
                    }
                }

                worker.GetComponent<Worker>().LoadWorkerData(workerData);
            }
        }
        else
        {
            Debug.LogWarning("workersCount is not saved!");
        }
    }

    private void SaveTime()
    {
        var gTime = gManager.GetGameTime();
        var timeString = JsonUtility.ToJson(gTime);

        Debug.Log(timeString);
        PlayerPrefs.SetString("time", timeString);
    }

    private void LoadTime()
    {
        if(PlayerPrefs.HasKey("time"))
        {
            string timeString = PlayerPrefs.GetString("time");
            GameTime gTime = JsonUtility.FromJson<GameTime>(timeString);

            gManager.LoadTime(gTime);
        }
        else
        {
            Debug.LogWarning("You have no previous saved time!");
        }
    }

    private void SaveFactoryData()
    {
        PlayerPrefs.SetFloat("RealMoney",gManager.FactoryMoney.GetValue());
        PlayerPrefs.SetFloat("HappyMoney",gManager.HappyMoney.GetValue());
        PlayerPrefs.SetFloat("FloorCost", gManager.GameConfigFile.FloorCost);

        Debug.Log("Saved " + gManager.FactoryMoney.GetValue() + " Factory Money." +
            "\n"+ gManager.HappyMoney.GetValue() +" Happy Money.");
    }

    private void LoadFactoryData()
    {
        if(PlayerPrefs.HasKey("RealMoney"))
            gManager.FactoryMoney.SetValue(PlayerPrefs.GetFloat("RealMoney"));

        if(PlayerPrefs.HasKey("HappyMoney"))
            gManager.HappyMoney.SetValue(PlayerPrefs.GetFloat("HappyMoney"));

        if (PlayerPrefs.HasKey("FloorCost"))
            gManager.GameConfigFile.FloorCost = PlayerPrefs.GetFloat("FloorCost");
    }

    private void LoadAll()
    {
        LoadWorkers();
        LoadFloors();
        LoadFactoryData();
        LoadTime();
    }

    private void SaveAll()
    {
        SaveFloors();
        SaveWorkers();
        SaveFactoryData();
        SaveTime();
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        if (save)
        {
#endif
            SaveAll();
#if UNITY_EDITOR
        }
#endif
    }

    public void ClearSave()
    {
        Debug.Log("Cleared all saves!");
        PlayerPrefs.DeleteAll();
    }

}

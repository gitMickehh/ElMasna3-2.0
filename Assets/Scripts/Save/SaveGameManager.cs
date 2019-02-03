using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SaveGameManager : MonoBehaviour
{
    public FloorList listOfFloors;
    private Floor firstFloor;

    [Header("Prefab")]
    public GameObject FloorPrefab;

    private void Start()
    {
        LoadFloors();
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
                GameObject floor = Instantiate(FloorPrefab,position,new Quaternion());

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

        //floor.FillSerializableFloor();
        //string floorJson = JsonUtility.ToJson(floor.savingFloor);
        //PlayerPrefs.SetString("floor0", floorJson);

        PlayerPrefs.SetInt("floorCount", listOfFloors.Items.Count);
    }

    public void ClearSave()
    {
        PlayerPrefs.DeleteAll();
    }

    private void SaveAll()
    {
        SaveFloors();
    }

    private void OnApplicationQuit()
    {
        SaveAll();

        //clear any runtime list here
        listOfFloors.Items.Clear();
    }

}

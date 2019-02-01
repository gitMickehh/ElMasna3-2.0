using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SaveGameManager : MonoBehaviour
{
    public FloorList listOfFloors;
    public Floor firstFloor;

    [Header("Prefab")]
    public GameObject FloorPrefab;

    private void Awake()
    {
        LoadFloors();
    }

    private void LoadFloors()
    {
        if (PlayerPrefs.HasKey("floorCount"))
        {
            int floorCount = PlayerPrefs.GetInt("floorCount");

            firstFloor.savingFloor = 
                JsonUtility.FromJson<SerializableFloor>(PlayerPrefs.GetString("floor0"));
            firstFloor.LoadFloor();

            for (int i = 1; i < floorCount; i++)
            {
                string retrievedJson = PlayerPrefs.GetString("floor" + i);

                //floor order isn't checked... We need to check it (later)
                listOfFloors.Items[i].savingFloor =
                    JsonUtility.FromJson<SerializableFloor>(retrievedJson);

                listOfFloors.Items[i].LoadFloor();
            }
        }
        else
        {
            Debug.Log("floor count is not saved");
        }

    }

    private void SaveFloors()
    {
        for (int i = 0; i < listOfFloors.Items.Count; i++)
        {
            listOfFloors.Items[i].FillSerializableFloor();
            string floorJson = JsonUtility.ToJson(listOfFloors.Items[i].savingFloor);

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

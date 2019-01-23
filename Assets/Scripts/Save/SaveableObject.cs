using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public enum ObjectType
//{
//    Factory, Floor, Machine, Worker
//}

public class SaveableObject : MonoBehaviour
{
    protected string save;

    //[SerializeField]
    //private ObjectType objectType;

    //public int ID;
    private string tag;



    // Start is called before the first frame update
    void Start()
    {
        tag = gameObject.tag;
        if (tag != "Machine")
        {
            Debug.Log("tag: " + tag);
            SaveGameManager.Instance.saveableObjects.Add(this);
        }
    }

    public void Save(int id)
    {
        switch (tag)
        {
            //case "Machine":
            //    Machine machine = GetComponent<Machine>();
            //    PlayerPrefs.SetString(Application.loadedLevel.ToString() + "-" + id.ToString(),
            //        tag + "_" + machine.hasWorker.ToString());
            //    break;

            case "Worker":
                Worker worker = GetComponent<Worker>();
                PlayerPrefs.SetString(Application.loadedLevel.ToString() + "-" + id.ToString(),
                    tag + "_" +
                    worker.ID.ToString() + "_" + worker.FullName + "_" +
                    worker.gender.ToString() + "_" + worker.level.ToString() + "_" +
                    /*worker.emotion.ToString() + "_" +*/ worker.happyMeter.ToString());
                break;

            case "Floor":
                Floor floor = GetComponent<Floor>();
                if (GetComponentInChildren<Machine>())
                {
                    Machine machine = GetComponentInChildren<Machine>();

                    PlayerPrefs.SetString(Application.loadedLevel.ToString() + "-" + id.ToString(),
                    tag + "_" + floor.noOfMachines.ToString() + "_" + machine.hasWorker.ToString());
                }
                else
                {
                    PlayerPrefs.SetString(Application.loadedLevel.ToString() + "-" + id.ToString(),
                    tag + "_" + floor.noOfMachines.ToString());
                }

                break;

            case "Factory":
                Factory factory = GetComponent<Factory>();
                PlayerPrefs.SetString(Application.loadedLevel.ToString() + "-" + id.ToString(),
                    tag + "_" +
                    factory.factoryMoney.ToString() + "_" + factory.noOfFloors.ToString());
                break;
        }

    }

    public void Load(string[] values)
    {
        switch (values[0])
        {
            //case "Machine":
            //    Machine machine = GetComponent<Machine>();
            //    Debug.Log(values[1]);
            //    machine.hasWorker = bool.Parse(values[1]);
            //    //Convert.ToBoolean(values[1]);
            //    break;

            case "Worker":
                Worker worker = GetComponent<Worker>();
                int.TryParse(values[1], out worker.ID);
                worker.FullName = values[2];
                //  worker.gender = (Gender)Enum.Parse(typeof(Gender), values[3]);
                worker.gender = (Gender)Enum.Parse(typeof(Gender), values[3]);
                //Enum.TryParse(values[3], out worker.gender);
                //worker.gender = Gender.Parse(values[3]);
                int.TryParse(values[4], out worker.level);
                float.TryParse(values[5], out worker.happyMeter);

                break;

            case "Floor":
                Floor floor = GetComponent<Floor>();
                if (GetComponentInChildren<Machine>())
                {
                    Machine machine = GetComponentInChildren<Machine>();
                    int.TryParse(values[1], out floor.noOfMachines);
                    machine.hasWorker = bool.Parse(values[2]);

                }
                else
                {
                    int.TryParse(values[1], out floor.noOfMachines);
                }

                break;

            case "Factory":
                Factory factory = GetComponent<Factory>();
                float.TryParse(values[1], out factory.factoryMoney);
                int.TryParse(values[2], out factory.noOfFloors);
                break;
        }
    }

    public void DestroySaveable()
    {
        SaveGameManager.Instance.saveableObjects.Remove(this);
        Destroy(gameObject);
    }

    void OnDisable()
    {
        if (Application.isPlaying)
            DestroySaveable();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SaveGameManager : MonoBehaviour
{
    private static SaveGameManager instance;

    public List<SaveableObject> saveableObjects
    {
        get;
        private set;
    }
    //public SaveableObject[] saveableObjectsInScene;
    //public List<SaveableObject> saveableObjectsInScene
    //{
    //    get
    //    {
    //        return saveableObjectsInScene;
    //    }
    //    private set { }
    //}

    public static SaveGameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<SaveGameManager>();

            return instance;
        }
        set
        {
            instance = value;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        saveableObjects = new List<SaveableObject>();
        //saveableObjectsInScene = new List<SaveableObject>();
    }
    //void Start()
    //{
    //    saveableObjectsInScene = (SaveableObject[])GameObject.FindObjectsOfType(typeof(SaveableObject));
    //}

    public void Save()
    {
        PlayerPrefs.SetInt(Application.loadedLevel.ToString(), saveableObjects.Count);

        for (int i = 0; i < saveableObjects.Count; i++)
        {
            saveableObjects[i].Save(i);  //int id
        }
    }

    public void Load()
    {
        foreach (SaveableObject obj in saveableObjects.ToList())
        {
            Debug.Log("Here");
            if(obj != null)
            {
                Destroy(obj.gameObject);
            }
            saveableObjects.Clear();
        }
        int objectCount = PlayerPrefs.GetInt(Application.loadedLevel.ToString()); 
        for (int i = 0; i < objectCount; i++)
        {
            //saveableObjects[i].Load();
            string[] value = PlayerPrefs.GetString(Application.loadedLevel +"-"+ i.ToString()).Split('_');

            GameObject tmp = null;

            switch (value[0])
            {
                ////case "Machine":
                ////    tmp = Instantiate(Resources.Load("Machine") as GameObject);
                ////    break;

                case "Worker":
                    tmp = Instantiate(Resources.Load("Worker") as GameObject);
                    break;

                case "Floor":
                    Debug.Log("Leh Ma3amalsh instantiate hena");
                    tmp = Instantiate(Resources.Load("Floor") as GameObject);
                    break;

                case "Factory":
                    tmp = Instantiate(Resources.Load("Factory") as GameObject);
                    break;
                    
                    Debug.Log("ra7 ll defalut leeh ya 3naya");
                   
            }

            if (tmp != null)
            {
                tmp.GetComponent<SaveableObject>().Load(value);
            }
        }
    }


}

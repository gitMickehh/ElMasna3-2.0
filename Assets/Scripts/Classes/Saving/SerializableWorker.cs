using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableWorker
{
    public int WorkerID;
    public int modelID;

    public int machineID;
    public bool inOrientation;

    public string fullName;
    public Gender gender;
    public int level;

    public int emotionalTrait;
    public int medicalTrait;

    public float happiness;

    public SerializableWorker()
    {
        WorkerID = 0;
        modelID = 1;
        machineID = -1;
        inOrientation = true;

        fullName = "standard";
        gender = Gender.MALE;
        level = 1;

        emotionalTrait = 1;
        medicalTrait = 1;
        happiness = 0.5f;
    }

    public SerializableWorker(int id, string name, Gender g = Gender.MALE, bool orientation = true, int model_id = 1, int lvl = 1, int emotionT = 1, int medicalT = 1, float happyMeter = 0.5f, int machID = -1)
    {
        WorkerID = id;
        modelID = model_id;
        machineID = machID;
        inOrientation = orientation;

        fullName = name;
        gender = g;
        level = lvl;

        emotionalTrait = emotionT;
        medicalTrait = medicalT;
        happiness = happyMeter;
    }

}

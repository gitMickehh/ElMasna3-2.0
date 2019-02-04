using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableWorker
{
    public int WorkerID;
    public int modelID;

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
        fullName = "standard";
        gender = Gender.MALE;
        level = 1;

        emotionalTrait = 1;
        medicalTrait = 1;
        happiness = 0.5f;
    }

    public SerializableWorker(int id, string name, Gender g = Gender.MALE, int model_id = 1, int lvl = 1, int emotionT = 1, int medicalT = 1, float happyMeter = 0.5f)
    {
        WorkerID = id;
        modelID = model_id;
        fullName = name;
        gender = g;
        level = lvl;

        emotionalTrait = emotionT;
        medicalTrait = medicalT;
        happiness = happyMeter;
    }

}

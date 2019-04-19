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
    public float happinessDefense;

    public float currentExperience;
    //public WorkerCustomizationSerializable CustomizationData;
    public int[] CustomizationDataIDs;

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
        happinessDefense = 0.2f;

        currentExperience = 0;

        //CustomizationData = new WorkerCustomizationSerializable();
        CustomizationDataIDs = new int[3] {-1,-1,-1};
    }

    public SerializableWorker(int id, string name, Gender g = Gender.MALE, bool orientation = true, int model_id = 1, int lvl = 1, int emotionT = 1, int medicalT = 1, float happyMeter = 0.5f, float happyDef = 0.2f, int machID = -1, float cExp= 0)
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
        currentExperience = cExp;

        happinessDefense = happyDef;
        CustomizationDataIDs = new int[3] { -1, -1, -1 };
    }

    /// <summary>
    /// <list type="bullet">
    ///     <item>0 for Head </item>
    ///     <item>1 for Face </item>
    ///     <item>2 for Body </item>
    /// </list>
    /// </summary>
    /// <param name="cDataArray"></param>
    public void AddCustomization(int[] cDataArray)
    {
        CustomizationDataIDs = cDataArray;
    }

    //public void AddCustomization(WorkerCustomizationSerializable cData)
    //{
    //    CustomizationData = cData;
    //}
}

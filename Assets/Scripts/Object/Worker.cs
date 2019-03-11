using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkerState
{
    Idle,
    Working,
    InMiniGame,
    Winning,
    Complaining,
    Walking
}

public class Worker : MonoBehaviour
{
    public int ID;
    public int modelID;
    public Machine currentMachine;
    public bool inOrientation;

    [Header("Management")]
    public WorkerInfo WorkerStats;
    public WorkerList listOfWorkers;

    [Header("Info")]
    public string FullName;
    public Gender gender;
    public int level;

    [Header("Traits")]
    public EmotionalTrait emotional;
    public MedicalTrait medical;
    public WorkerState workerState;

    [Header("Happiness")]
    [Range(0, 100)]
    public float happyMeter;
    public float happyDefense;

    [Header("Model")]
    [HideInInspector]
    public SkinnedMeshRenderer[] skinnedMeshRenderers;
    public WorkerCustomization customization;

    [Header("UI Camera")]
    public Transform UICameraPosition;

    Animator workerAnimator;

    //saving
    public SerializableWorker WorkerData {
        get { return GetWorkerData(); }
    }

    void Awake()
    {
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        workerAnimator = GetComponentInChildren<Animator>();
        customization = GetComponent<WorkerCustomization>();
    }

    private void OnEnable()
    {
        ID = listOfWorkers.GetNewId();
        listOfWorkers.Add(this);
    }

    private void OnDisable()
    {
        listOfWorkers.Remove(this);
    }

    public void RandomizeWorker()
    {
        inOrientation = true;

        FullName = WorkerStats.RandomizeName(gender);
        emotional = WorkerStats.RandomizeEmotionalTraits();
        medical = WorkerStats.RandomizeMedicalTraits();

        happyMeter = 50;
        happyDefense = Random.Range(0.0f,1.0f);
        level = 0;

        transform.name = FullName;
    }

    public void LevelUp()
    {
        Debug.Log(transform.name + " leveled Up!");
        level++;

        //if (coolDownTime != 0.1)
        //{
        //    coolDownTime -= decreaseCoolDownTimeBy;
        //}

        ////when adding machine class 
        //workingSpeed += increaseWorkingSpeedBy;
    }

    public override string ToString()
    {
        return FullName + ", Gender: " + gender.ToString()
            + "\n level: " + level;
    }

    public void AddHappiness(float percentage)
    {
        happyMeter = Mathf.Clamp(happyMeter + percentage, 0, 100);
    }

    public void DecreaseHappiness(float percentage)
    {
        happyMeter = Mathf.Clamp(happyMeter - percentage, 0, 100);
    }

    public void SetWorkerState(WorkerState state)
    {
        workerState = state;
        switch (state)
        {
            case WorkerState.Idle:
                workerAnimator.SetBool("Working", false);
                break;

            case WorkerState.Working:
                workerAnimator.SetBool("Working", true);
                break;

            case WorkerState.Winning:
                workerAnimator.SetBool("Working", false);
                workerAnimator.SetTrigger("WinTrigger");
                break;

            case WorkerState.Complaining:
                GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.black;
                break;

        }
    }

    public SerializableWorker GetWorkerData()
    {
        SerializableWorker sw;

        if (currentMachine != null)
            sw = new SerializableWorker(ID, FullName, gender, inOrientation,modelID, level, emotional.no, medical.no, happyMeter, currentMachine.machineID);
        else
            sw = new SerializableWorker(ID, FullName, gender, inOrientation, modelID, level, emotional.no, medical.no, happyMeter);

        return sw;
    }

    public void LoadWorkerData(SerializableWorker wData)
    {
        ID = wData.WorkerID;
        modelID = wData.modelID;
        //currentMachine = listofMachines.GetMachineByID(wData.machineID);
        inOrientation = wData.inOrientation;

        FullName = wData.fullName;
        gender = wData.gender;
        level = wData.level;

        emotional = WorkerStats.GetEmotionalTraitByNumber(wData.emotionalTrait);
        medical = WorkerStats.GetMedicalTraitByNumber(wData.medicalTrait);

        happyMeter = wData.happiness;

        transform.name = FullName;
    }


}
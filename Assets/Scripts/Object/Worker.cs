using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkerState
{
    Idle,
    Working,
    InBreak,
    Winning,
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

    public float healthMeter;

    [Header("Experience")]
    public float currentExperience;
    public float maxExperience;

    [Header("Model")]
    [HideInInspector]
    public SkinnedMeshRenderer[] skinnedMeshRenderers;
    public WorkerCustomization customization;

    [Header("UI Camera")]
    public Transform UICameraPosition;

    public Animator workerAnimator;

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

        happyMeter = 100;
        healthMeter = medical.start;

        happyDefense = Random.Range(0.0f,1.0f);
        level = 0;
        maxExperience = 50;
        currentExperience = 0;

        transform.name = FullName;
    }

    public void GetNewName()
    {
        FullName = WorkerStats.RandomizeName(gender);
        transform.name = FullName;
    }

    public void LevelUp()
    {
        Debug.Log(transform.name + " leveled Up!");
        level++;
    }

    public override string ToString()
    {
        return FullName + ", Gender: " + gender.ToString()
            + "\n level: " + level;
    }

    public void ModifyHappiness()
    {
        switch (workerState)
        {
            case WorkerState.Working:
            happyMeter = (happyMeter - 0.5f);
                break;
            case WorkerState.InBreak:
                happyMeter = (happyMeter + 4);
                break;
            default:
                break;
        }

        happyMeter = Mathf.Clamp(happyMeter, 0, 100);
    }

    public void ModifyHealth()
    {
        switch (workerState)
        {
            case WorkerState.Working:
                healthMeter = (healthMeter - medical.rate);
                break;
            case WorkerState.InBreak:
                healthMeter = (healthMeter + 2* medical.rate);
                break;
            default:
                break;
        }

        healthMeter = Mathf.Clamp(healthMeter, 0, 100);
    }

    public void ModifyExperience()
    {
        if(currentMachine != null && currentMachine.IsWorking && !currentMachine.isWaiting)
            currentExperience++;

        if(currentExperience >= maxExperience)
        {
            LevelUp();
            currentExperience = 0;
            maxExperience = level * 50 + 50;
        }
    }

    public void AddHappiness(float percentage)
    {
        happyMeter = Mathf.Clamp(happyMeter + percentage, 0, 100);
        workerAnimator.SetFloat("Happiness", happyMeter / 100.0f);
    }

    public void DecreaseHappiness(float percentage)
    {
        happyMeter = Mathf.Clamp(happyMeter - percentage, 0, 100);
        workerAnimator.SetFloat("Happiness", happyMeter / 100.0f);

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
                //workerAnimator.SetBool("Working", false);
                workerAnimator.SetTrigger("Winning");
                break;

            //case WorkerState.Walking:
            //    workerAnimator.SetBool("Walking", true);
            //    break;
            default:
                break;
        }
    }

    public void SetBreak(BreakObject breakObject)
    {
        workerState = WorkerState.InBreak;

        switch (breakObject)
        {
            case BreakObject.Sit:
                workerAnimator.SetBool("Sit", true);
                break;

            case BreakObject.Talk:
                workerAnimator.SetBool("Talking", true);
                break;

        }       
    }

    public void SetWorking(bool state)
    {
        workerAnimator.SetBool("Working", state);
        if (state)
            workerState = WorkerState.Working;
        else
            workerState = WorkerState.InBreak;
    }

    public void SetWalking(bool state)
    {
        workerAnimator.SetBool("Walking", state);
        //SetWorking(false);
    }


    public SerializableWorker GetWorkerData()
    {
        SerializableWorker sw;

        if (currentMachine != null)
            sw = new SerializableWorker(ID, FullName, gender, inOrientation,modelID, level, emotional.no, medical.no, happyMeter,happyDefense, currentMachine.machineID);
        else
            sw = new SerializableWorker(ID, FullName, gender, inOrientation, modelID, level, emotional.no, medical.no, happyMeter, happyDefense);

        sw.AddCustomization(customization.GetCustomizationDataArray());
        //sw.AddCustomization(customization.GetCustomizationData());
        sw.currentExperience = currentExperience;
        sw.currentHealth = healthMeter;
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
        happyDefense = wData.happinessDefense;

        healthMeter = wData.currentHealth;

        customization.LoadCustomizationData(wData.CustomizationDataIDs);

        currentExperience = wData.currentExperience;
        maxExperience = level * 50 + 50;

        transform.name = FullName;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkerState
{
    Idle,
    Working,
    InMiniGame,
    Winning,
    Complaining
}

public class Worker : MonoBehaviour
{
    public int ID;

    [Header("Randomnes")]
    public bool isRandom = true;

    [Header("Management")]
    public WorkerInfo WorkerStats;
    public WorkerState workerState;

    [Header("Info")]
    public string FullName;
    public Gender gender;
    public int level;
    //title for promotion?

    [Header("Traits")]
    public EmotionalTrait emotion;
    public MedicalTrait medicalState;

    //public bool workingState;

    [Range(0, 100)]
    public float happyMeter;

    [Header("Cooldown")]
    public float coolDownTime;
    public float decreaseCoolDownTimeBy = 0.01f;

    [Header("Speed")]
    public float movementSpeed;
    public float workingSpeed;
    public float increaseWorkingSpeedBy = 0.05f;

    [Header("Model")]
    public SkinnedMeshRenderer[] skinnedMeshRenderers;


    //[Header("Scriptable Objects")]
    //public Factory_SO factory_SO;

    public Animator workerAnimator;
    public Machine machineAssigned;

    public float PlayingToLevelIndivCost
    {
        get
        {
            return 200 * Mathf.Pow(1.05f, level);
        }
    }

    void Awake()
    {
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        workerAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (isRandom)
        {
            //GenerateWorker();
            isRandom = false;
            workerState = WorkerState.Idle;
        }
        else
        {
            //load worker
        }
    }

    public void GenerateWorker()
    {
        //gender = WorkerStats.RandomizeGender();
        FullName = WorkerStats.RandomizeName(gender);

        emotion = WorkerStats.RandomizeEmotionalTraits();
        medicalState = WorkerStats.RandomizeMedicalTraits();

        coolDownTime = WorkerStats.CooldownTime;
        movementSpeed = WorkerStats.MovementSpeed;

        happyMeter = 50;
        level = 0;
        workingSpeed = 1f;

        //shirt color
        //var workerColor = WorkerStats.RandomizeColorLinker();

        //for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        //{
        //    skinnedMeshRenderers[i].material = workerColor.WorkerMaterial;
        //}
        transform.name = FullName;
    }

    //public void AddComplaint()
    //{
    //    SetWorkerState(WorkerState.Complaining);
    //    Complaints_SO comp = complaintsManager_SO.GenerateRandomWorkerComplaint();
    //    complaints.Add(comp);
    //}

    //public void RemoveComplaint(Complaints_SO comp)
    //{
    //    if (complaints.Contains(comp))
    //    {
    //        complaints.Remove(comp);
    //    }
    //}

    public void LevelUp()
    {
        print("LevelUp Event has been invoked");
        level++;

        if (coolDownTime != 0.1)
        {
            coolDownTime -= decreaseCoolDownTimeBy;
        }

        //when adding machine class 
        workingSpeed += increaseWorkingSpeedBy;
    }

    //public void AssignWorker(Machine machine)
    //{
    //    machineAssigned = machine;
    //    machineAssigned.worker = this;
    //    transform.position = machineAssigned.workerPosition.position;
    //    transform.rotation = machineAssigned.workerPosition.rotation;

    //    machineAssigned.SetMachineState(MachineState.Working);

    //    if (workerState != WorkerState.InMiniGame)
    //        SetWorkerState(WorkerState.Working);

    //}

    public override string ToString()
    {
        return FullName + ", Gender: " + gender.ToString()
            + "\n level: " + level;
    }

    //happiness
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
                //print("winning");
                workerAnimator.SetBool("Working", false);
                workerAnimator.SetTrigger("WinTrigger");
                //StartCoroutine(WaitTillWinningFinish());
                //if ((machineAssigned) && (machineAssigned.machineState == MachineState.Broken))
                //{
                //    workerAnimator.SetBool("Working", false);
                //    workerState = WorkerState.Idle;
                //}
                //else
                //{
                //    workerAnimator.SetBool("Working", true);
                //    workerState = WorkerState.Working;
                //}
                break;

            case WorkerState.Complaining:
                GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.black;
                break;

        }
    }

    //IEnumerator WaitTillWinningFinish()
    //{
    //    yield return new WaitForSeconds(3.5f);

    //   //WorkerStats.disappearingParticles.Play();

    //    if (workerManager.workersInOrientation.Contains(gameObject))
    //    {
    //        gameObject.SetActive(false);
    //    }
    //}

    //public void PlayerWon()
    //{
    //    SetWorkerState(WorkerState.Winning);
    //}

    //public void SaveWorker()
    //{
    //    SaveSystem.SaveWorker(this);
    //}

    //public void LoadWorker()
    //{
    //    WorkerData data = SaveSystem.LoadWorker();

    //    ID = data.ID;
    //    FullName = data.FullName;
    //    gender = (Gender)data.gender;
    //    level = data.level;
    //    //emotion = (EmotionalTrait)WorkerStats.EmotionalTraits.ListElements[data.emotion];
    //    happyMeter = data.happyMeter;

    //}

}
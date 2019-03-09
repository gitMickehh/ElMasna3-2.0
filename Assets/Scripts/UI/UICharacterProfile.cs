using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICharacterProfile : MonoBehaviour
{
    public GameConfig gameConfigFile;
    private int workerID;

    [Header("UI Objects")]
    public Text CharacterName;
    public Text CharacterLevel;
    public Slider CharacterHappienss;
    public Slider CharacterHealth;
    public GameObject InOrientationButtons;
    public GameObject InFactoryButtons;

    //[Header("UI Buttons")]
    //public Button HireButton;
    //public Button CustomizeButton;
    //public Button ConverseButton;
    //public Button BreakButton;

    bool opened;

    [Header("Camera")]
    public GameObject CameraPrefab;
    [Attributes.GreyOut]
    [SerializeField]
    private Camera UICamera;

    [Header("Worker")]
    public WorkerField SelectedWorkerRefernce;

    [Header("Animator")]
    public Animator animator;

    //Modal Panel Options
    private ModalPanel modalPanel;
    private UnityAction HireAction;
    private UnityAction GiveBreakAction;

    private void Start()
    {
        UICamera = Instantiate(CameraPrefab, new Vector3(), new Quaternion()).GetComponent<Camera>();
        modalPanel = ModalPanel.Instance();

        HireAction = new UnityAction(HireWorker);
        GiveBreakAction = new UnityAction(GiveBreakToWorker);
    }

    private void FillWorkerData(Worker w)
    {
        workerID = w.ID;

        CharacterName.text = w.FullName;
        CharacterLevel.text = w.level.ToString();
        CharacterHappienss.value = (w.happyMeter / 100.0f);

        if (w.inOrientation)
        {
            InOrientationButtons.SetActive(true);
            InFactoryButtons.SetActive(false);
        }
        else
        {
            InFactoryButtons.SetActive(true);
            InOrientationButtons.SetActive(false);
        }

        UICamera.transform.SetParent(w.UICameraPosition);
        UICamera.transform.localPosition = new Vector3();
        UICamera.transform.rotation = new Quaternion();
    }

    public void ClosePanel()
    {
        if (opened)
        {
            animator.SetTrigger("TriggerUI");
            opened = false;
            SelectedWorkerRefernce.worker = null;
        }
    }

    public void GetWorkerProfile()
    {
        if(workerID != SelectedWorkerRefernce.worker.ID)
        {
            FillWorkerData(SelectedWorkerRefernce.worker);

            if (!opened)
                animator.SetTrigger("TriggerUI");

            opened = true;
        }
    }

    public void HireClick()
    {
        string s = gameConfigFile.CurrentLanguageProfile.HireWorker + gameConfigFile.CurrentLanguageProfile.QuestionMark;
        modalPanel.Choice(s, HireAction);
    }

    public void GiveBreakClick()
    {
        string s = gameConfigFile.CurrentLanguageProfile.GiveWorkerBreak + gameConfigFile.CurrentLanguageProfile.QuestionMark;
        modalPanel.Choice(s, GiveBreakAction);
    }

    private void HireWorker()
    {
        Debug.Log("Hiring " + SelectedWorkerRefernce.worker.FullName);
        //SelectedWorkerRefernce.worker.Hire();
    }

    private void GiveBreakToWorker()
    {
        Debug.Log("Giving a break to " + SelectedWorkerRefernce.worker.FullName);
        //SelectedWorkerRefernce.worker.Break();
    }



}

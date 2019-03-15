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

    bool opened;

    [Header("Camera")]
    public GameObject CameraPrefab;
    [Attributes.GreyOut]
    [SerializeField]
    private Camera UICamera;

    public WorkerCustomizationPanel customizationPanel;

    [Header("Worker")]
    public WorkerField SelectedWorkerRefernce;

    [Header("Animator")]
    public Animator animator;

    [Header("Events")]
    public GameEvent HireSelectedWorker;

    //Modal Panel Options
    private ModalPanel modalPanel;
    private UnityAction GiveBreakAction;

    private void Start()
    {
        UICamera = Instantiate(CameraPrefab, new Vector3(), new Quaternion()).GetComponent<Camera>();
        modalPanel = ModalPanel.Instance();

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
        if (workerID != SelectedWorkerRefernce.worker.ID || !opened)
        {
            FillWorkerData(SelectedWorkerRefernce.worker);

            if (!opened)
                animator.SetTrigger("TriggerUI");

            opened = true;
        }
    }

    public void CustomizationPanel()
    {
        customizationPanel.gameObject.SetActive(true);
    }

    public void GiveBreakClick()
    {
        string s = gameConfigFile.CurrentLanguageProfile.GiveWorkerBreak + gameConfigFile.CurrentLanguageProfile.QuestionMark;
        modalPanel.Choice(s, GiveBreakAction);
    }

    private void GiveBreakToWorker()
    {
        Debug.Log("Giving a break to " + SelectedWorkerRefernce.worker.FullName);
        //SelectedWorkerRefernce.worker.Break();
    }

}

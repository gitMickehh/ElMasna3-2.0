using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICharacterProfile : MonoBehaviour
{
    [Header("UI Objects")]
    public Text CharacterName;
    public Text CharacterLevel;
    public Slider CharacterHappienss;
    public Slider CharacterHealth;
    public GameObject InOrientationButtons;
    public GameObject InFactoryButtons;

    [Header("UI Buttons")]
    public Button HireButton;
    public Button CustomizeButton;
    public Button ConverseButton;
    public Button BreakButton;

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

    private void Start()
    {
        UICamera = Instantiate(CameraPrefab, new Vector3(), new Quaternion()).GetComponent<Camera>();
        modalPanel = ModalPanel.Instance();

        HireAction = new UnityAction(HireWorker);
    }

    private void FillWorkerData(Worker w)
    {
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
            animator.SetTrigger("TriggerUI");
    }

    public void GetWorkerProfile()
    {
        FillWorkerData(SelectedWorkerRefernce.worker);

        if (!opened)
            animator.SetTrigger("TriggerUI");

        opened = true;
    }

    public void HireClick()
    {
        modalPanel.Choice("Do you want to hire this?", HireAction);
    }

    public void HireWorker()
    {
        Debug.Log("Hiring " + SelectedWorkerRefernce.worker.FullName);
        //SelectedWorkerRefernce.worker.Hire();
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    bool opened;

    [Header("Camera")]
    public GameObject CameraPrefab;
    public Camera UICamera;

    [Header("Worker")]
    public WorkerField SelectedWorkerRefernce;

    [Header("Animator")]
    public Animator animator;

    private void Start()
    {
        UICamera = Instantiate(CameraPrefab,new Vector3(), new Quaternion()).GetComponent<Camera>();
    }

    private void FillWorkerData(Worker w)
    {
        CharacterName.text = w.FullName;
        CharacterLevel.text = w.level.ToString();
        CharacterHappienss.value = (w.happyMeter/100.0f);

        if(w.inOrientation)
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
        if(opened)
            animator.SetTrigger("TriggerUI");
    }

    public void GetWorkerProfile()
    {
        FillWorkerData(SelectedWorkerRefernce.worker);

        if(!opened)
            animator.SetTrigger("TriggerUI");

        opened = true;
    }
}

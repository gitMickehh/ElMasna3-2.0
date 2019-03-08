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

    bool opened;

    [Header("Camera")]
    public Camera UICamera;

    [Header("Worker")]
    public WorkerField SelectedWorkerRefernce;

    [Header("Animator")]
    public Animator animator;

    private void FillWorkerData(Worker w)
    {
        CharacterName.text = w.FullName;
        CharacterLevel.text = w.level.ToString();
        CharacterHappienss.value = (w.happyMeter/100.0f);

        //UICamera.transform.SetParent(w.UICameraPosition);
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

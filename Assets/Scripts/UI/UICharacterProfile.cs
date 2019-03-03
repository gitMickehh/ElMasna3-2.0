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

    [Header("Camera")]
    public Camera UICamera;

    //[Header("Worker")]
    //[Attributes.GreyOut]
    //public Worker WorkerRefernce;

    public void FillInWorkerData(Worker w)
    {
        CharacterName.text = w.FullName;
        CharacterLevel.text = w.level.ToString();
        CharacterHappienss.value = (w.happyMeter/100.0f);


    }
}

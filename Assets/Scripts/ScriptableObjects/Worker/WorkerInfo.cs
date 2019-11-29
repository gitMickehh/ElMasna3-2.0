using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkerStats", menuName = "ElMasna3/Worker Data")]
public class WorkerInfo : ScriptableObject
{

    [Header("Names")]
    public GameConfig gameConfigFile;

    public ListOfStrings MaleFirstNames {
        get { return gameConfigFile.CurrentLanguageProfile.MaleNames; }
    }

    public ListOfStrings FemaleFirstNames {
        get { return gameConfigFile.CurrentLanguageProfile.FemaleNames; }
    }

    public ListOfStrings LastNames {
        get { return gameConfigFile.CurrentLanguageProfile.LastNames; }
    }

    [Header("Traits")]
    public ScriptableObjectsList EmotionalTraits;
    public ScriptableObjectsList MedicalTraits;

    [Header("Leveling up")]
    public float workSpeedRate;
    [Tooltip("This makes the older workers less likely to be sad")]
    public float happinessDefenseFactor;

    [Header("Prefabs")]
    [Tooltip("This is going to be it's own prefab list and all")]
    public List<GameObject> MalePrefabs;
    public List<GameObject> FemalePrefabs;

    public List<GameObject> HairPrefabs;
    public List<GameObject> FacialHairPrefabs;
    public List<GameObject> GlassesPrefabs;

    [Header("Particles")]
    public GameObject disappearingParticles;

    [Header("Emotions UI")]
    public float EmotionUIOnTime = 2f;

    public string RandomizeName(Gender g)
    {
        return gameConfigFile.CurrentLanguageProfile.GetRandomFullName(g);
    }

    public EmotionalTrait RandomizeEmotionalTraits()
    {
       int no1 = Random.Range(0, EmotionalTraits.ListElements.Count);

        return (EmotionalTrait)(EmotionalTraits.ListElements)[no1];
    }

    public MedicalTrait RandomizeMedicalTraits()
    {
        int no1 = Random.Range(0, MedicalTraits.ListElements.Count);

        return (MedicalTrait)(MedicalTraits.ListElements)[no1];
    }

    public Gender RandomizeGender()
    {
        int no1 = Random.Range(0, 2);

        return (Gender)no1;
    }
   
    public GameObject GetWorkerPrefab(Gender g)
    {
        GameObject model;
        int r;
        switch (g)
        {
            case Gender.MALE:
                r = Random.Range(0,MalePrefabs.Count);
                model = MalePrefabs[r];
                break;
            case Gender.FEMALE:
                r = Random.Range(0, FemalePrefabs.Count);
                model = FemalePrefabs[r];
                break;
            default:
                r = Random.Range(0, MalePrefabs.Count);
                model = MalePrefabs[r];
                break;
        }

        return model;
    }

    public Gender GetGender()
    {
        int r = Random.Range(0,2);
        return (Gender)r;
    }

    public EmotionalTrait GetEmotionalTraitByNumber(int n)
    {
        for (int i = 0; i < EmotionalTraits.ListElements.Count; i++)
        {
            var et = (EmotionalTrait)EmotionalTraits.ListElements[i];

            if (et.no == n)
                return et;
        }

        return (EmotionalTrait)EmotionalTraits.ListElements[0];
    }

    public MedicalTrait GetMedicalTraitByNumber(int n)
    {
        for (int i = 0; i < EmotionalTraits.ListElements.Count; i++)
        {
            var mt = (MedicalTrait)MedicalTraits.ListElements[i];

            if (mt.no == n)
                return mt;
        }

        return (MedicalTrait)EmotionalTraits.ListElements[0];
    }
}

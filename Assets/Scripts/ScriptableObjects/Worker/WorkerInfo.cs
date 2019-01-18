using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkerStats", menuName = "ElMasna3/Worker Data")]
public class WorkerInfo : ScriptableObject
{
    
    [Header("Names")]
    public ListOfStrings MaleFirstNames;
    public ListOfStrings FemaleFirstNames;
    public ListOfStrings LastNames;

    [Header("Traits")]
    public ScriptableObjectsList EmotionalTraits;
    public ScriptableObjectsList MedicalState;
    //Favorite day will be an ENUM randomly generated in Worker Component

    [Header("Colors")]
    public ScriptableObjectsList ShirtColorsLinks;

    [Header("Cooldown")]
    public float CooldownTime;
    public float MovementSpeed;

    [Header("Prefabs")]
    public List<GameObject> MalePrefabs;
    public List<GameObject> FemalePrefabs;

    public List<GameObject> HairPrefabs;
    public List<GameObject> FacialHairPrefabs;
    public List<GameObject> GlassesPrefabs;

    [Header("Particles")]
    public GameObject disappearingParticles;

    public string RandomizeName(Gender g)
    {
        int no1 = 0;
        int no2 = Random.Range(0, LastNames.strings.Count);

        string fullName;

        switch (g)
        {
            case Gender.MALE:
                no1 = Random.Range(0, MaleFirstNames.strings.Count);
                
                //english
                //fullName = MaleFirstNames.strings[no1] + " " + LastNames.strings[no2];
                
                //arabic:
                fullName = LastNames.strings[no2] + " " + MaleFirstNames.strings[no1];
                break;
            case Gender.FEMALE:
                no1 = Random.Range(0, FemaleFirstNames.strings.Count);

                //english
                //fullName = FemaleFirstNames.strings[no1] + " " + LastNames.strings[no2];

                //arabic:
                fullName = LastNames.strings[no2] + " " + FemaleFirstNames.strings[no1];
                break;
            default:
                fullName = "Hamada Hamda";
                break;
        }

        return fullName;

    }

    public EmotionalTrait RandomizeEmotionalTraits()
    {
       int no1 = Random.Range(0, EmotionalTraits.ListElements.Count);

        return (EmotionalTrait)(EmotionalTraits.ListElements)[no1];
    }

    public MedicalTrait RandomizeMedicalTraits()
    {
        int no1 = Random.Range(0, MedicalState.ListElements.Count);

        return (MedicalTrait)(MedicalState.ListElements)[no1];
    }

    //public Day GetRandomFavDay()
    //{
    //    int no1 = Random.Range(0, 7);

    //    return (Day)no1;
    //}

    public Gender RandomizeGender()
    {
        int no1 = Random.Range(0, 2);

        return (Gender)no1;
    }
   
    //public Color RandomizeColor()
    //{
    //    int r = Random.Range(0,ShirtColorsLinks.ListElements.Count);

    //    MiniGameLinker_SO s = (MiniGameLinker_SO)ShirtColorsLinks.ListElements[r];
    //    return s.ShirtColor;
    //}

    //public MiniGameLinker_SO RandomizeColorLinker()
    //{
    //    int r = Random.Range(0, ShirtColorsLinks.ListElements.Count);

    //    MiniGameLinker_SO s = (MiniGameLinker_SO)ShirtColorsLinks.ListElements[r];
    //    return s;
    //}

    /*
    public void RandomizeWorkerStats()
    {

    }

    public void RandomizeWorkerLook()
    {

    }
    */

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


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePanel : MonoBehaviour
{
    [Header("Holder")]
    public GameObjectField ObjectToBuy;

    [Header("Store Scheme")]
    public StoreScriptableObject storeScheme;

    [Header("UI")]
    public GameObject storeItemHolder;
    public Text PartyCostText;
    public GameObject MachineStorePage;

    [Header("Events")]
    public GameEvent ToPlaceMachine;

    public void Start()
    {
        FillInMachines();
    }

    private void FillInMachines()
    {
        for (int i = 0; i < storeScheme.Machines.Length; i++)
        {
            var sI = Instantiate(storeItemHolder, MachineStorePage.transform);
            sI.GetComponent<UIStoreObject>().FillInObject(storeScheme.Machines[i], storeScheme.RealMoneyIcon, this);
        }

        MachineStorePage.SetActive(false);
    }

    public void BuyObject(UIStoreObject sObject)
    {
        ObjectToBuy.gameObjectReference = sObject.ReferencePrefab;
        ToPlaceMachine.Raise();
        MachineStorePage.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}

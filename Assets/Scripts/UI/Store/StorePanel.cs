using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StorePanel : MonoBehaviour
{
    [Header("Holder")]
    public GameObjectField ObjectToBuy;
    UIStoreObject UIobjectToBuy;

    [Header("Store Scheme")]
    public StoreScriptableObject storeScheme;

    [Header("UI")]
    public GameObject storeItemHolder;
    public Text PartyCostText;
    public GameObject MachineStorePage;

    [Header("Events")]
    public GameEvent ToPlaceMachine;

    [Header("Game Manager")]
    [Attributes.GreyOut]
    public GameManager gameManager;
    public FloatField currentCheck;

    //Modal Panel
    private ModalPanel modalPanel;
    private UnityAction myYesAction;
    private UnityAction myCancelAction;

    public void Start()
    {
        FillInMachines();
        gameManager = FindObjectOfType<GameManager>();

        modalPanel = ModalPanel.Instance();

        myYesAction = new UnityAction(ConfirmBuy);
        myCancelAction = new UnityAction(CancelBuy);

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
        UIobjectToBuy = sObject;
        ObjectToBuy.gameObjectReference = UIobjectToBuy.ReferencePrefab;

        LanguageProfile lang = gameManager.GameConfigFile.CurrentLanguageProfile;
        string[] qs = new string[]
        {
            lang.YouWillPay,
            UIobjectToBuy.itemCost.ToString()
        };

        string message = lang.GetQuestion(qs);

        modalPanel.Choice(message,myYesAction,myCancelAction);
    }
    
    public void ConfirmBuy()
    {
        if (gameManager.CheckBalance(UIobjectToBuy.itemCost, UIobjectToBuy.currency))
        {
            //withdraw the money when you actually place the machine 
            //so if the player quits before this they don't lose their money
            //gameManager.WithdrawMoney(UIobjectToBuy.itemCost, UIobjectToBuy.currency);
            ToPlaceMachine.Raise();
            currentCheck.SetValue(UIobjectToBuy.itemCost);
            MachineStorePage.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("price is too high");
        }
    }

    public void CancelBuy()
    {
        UIobjectToBuy = null;
    }
}

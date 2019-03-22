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
    public GameEvent StartParty;

    [Header("Game Manager")]
    [Attributes.GreyOut]
    public GameManager gameManager;
    public FloatField currentCheck;

    //Modal Panel
    private ModalPanel modalPanel;
    private UnityAction BuyMachineAction;
    private UnityAction myCancelAction;

    public void Start()
    {
        FillInMachines();
        gameManager = FindObjectOfType<GameManager>();
        PartyCostText.text = gameManager.GameConfigFile.PartyCost.ToString();

        modalPanel = ModalPanel.Instance();

        BuyMachineAction = new UnityAction(ConfirmBuy);
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

        modalPanel.Choice(message,BuyMachineAction,myCancelAction);
    }
    
    public void ConfirmBuy()
    {
        if (gameManager.CheckBalance(UIobjectToBuy.itemCost, UIobjectToBuy.currency))
        {
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

    public void PartyOn()
    {
        LanguageProfile lang = gameManager.GameConfigFile.CurrentLanguageProfile;

        if (!gameManager.CheckBalance(gameManager.GameConfigFile.PartyCost,Currency.HappyMoney))
        {
            modalPanel.Message(lang.NotEnoughMoney,gameManager.GameConfigFile.icons[1]);
            return;
        }

        string[] Statement = new string[]
        {
            lang.GetQuestion(lang.DoYouWantToStartTheParty),
            "\n",
            lang.YouWillPay,
            PartyCostText.text
        };

        string message =  lang.GetStatement(Statement);

        modalPanel.Choice(message, new UnityAction(PartyPayAccept), myCancelAction, gameManager.GameConfigFile.icons[1]);
    }


    public void PartyPayAccept()
    {
        StartParty.Raise();
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICustomizationObject : MonoBehaviour
{
    [Header("Representation")]
    public Image itemImage;
    public Text itemCostText;
    public float itemCost;
    public Image currencyImage;

    [Header("InGame")]
    [Attributes.GreyOut]
    public GameObject ReferencePrefab;

    [Attributes.GreyOut]
    public string description;

    [Attributes.GreyOut]
    public WorkerCustomizationPanel customiztionPanel;

    private CustomizationItem myItem;

    public void ClickPreview()
    {
        customiztionPanel.workerSelected.worker.customization.PreviewItem(myItem);
        customiztionPanel.SetDirty(true);
    }

    public void FillInObject(CustomizationItem sItem, WorkerCustomizationPanel c, float money, CustomizationTier tierOfWorker)
    {
        myItem = sItem;

        itemImage.sprite = sItem.UIicon;
        itemCost = sItem.price;
        itemCostText.text = sItem.price.ToString("0");

        ReferencePrefab = sItem.item;
        customiztionPanel = c;

        bool show = false;
        switch (sItem.tier)
        {
            case CustomizationTier.TIER1:
                show = true;
                break;
            case CustomizationTier.TIER2:
                if (tierOfWorker == CustomizationTier.TIER2 || tierOfWorker == CustomizationTier.TIER3)
                    show = true;
                break;
            case CustomizationTier.TIER3:
                if (tierOfWorker == CustomizationTier.TIER3)
                    show = true;
                break;
            default:
                break;
        }

        if(money >= sItem.price && show)
        {
            itemImage.GetComponent<Button>().interactable = true;
            currencyImage.color = Color.white;
            itemCostText.color = Color.black;
        }
        else
        {
            itemImage.GetComponent<Button>().interactable = false;
            currencyImage.color = Color.grey;
            itemCostText.color = Color.grey;
        }
    }
}

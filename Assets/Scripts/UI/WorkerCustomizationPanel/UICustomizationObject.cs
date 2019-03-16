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

    public void FillInObject(CustomizationItem sItem, WorkerCustomizationPanel c)
    {
        myItem = sItem;

        itemImage.sprite = sItem.UIicon;
        itemCost = sItem.price;
        itemCostText.text = sItem.price.ToString("0");

        ReferencePrefab = sItem.item;
        customiztionPanel = c;
    }
}

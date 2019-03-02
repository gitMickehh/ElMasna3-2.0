using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStoreObject : MonoBehaviour
{
    [Header("Representation")]
    public Image itemImage;
    public Image currencyImage;
    public Text itemCost;

    [Header("InGame")]
    [Attributes.GreyOut]
    public GameObject ReferencePrefab;

    [Attributes.GreyOut]
    public string description;

    [Attributes.GreyOut]
    public StorePanel storePanel;


    public void FillInObject(StoreItem sItem, Sprite machineImage,StorePanel s)
    {
#if UNITY_EDITOR
        transform.name = sItem.name;
#endif

        itemImage.sprite = sItem.storeIcon;
        itemCost.text = sItem.price.ToString();
        currencyImage.sprite = machineImage;

        description = sItem.description;

        ReferencePrefab = sItem.Prefab;
        storePanel = s;
    }

    public void BuyItem()
    {
        storePanel.BuyObject(this);
    }
}

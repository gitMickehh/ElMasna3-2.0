using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorkerCustomizationSerializable {
    public CustomizationItem HeadItem;
    public CustomizationItem FaceItem;
    public CustomizationItem BodyItem;

    public WorkerCustomizationSerializable(CustomizationItem H, CustomizationItem F, CustomizationItem B)
    {
        HeadItem = H;
        FaceItem = F;
        BodyItem = B;
    }
}

public class WorkerCustomization : MonoBehaviour
{
    public GameObject SpineBone;

    public Transform HeadPlace;
    public Transform BodyPlace;
    private bool dirty;

    [Header("Customization Scheme")]
    public GameObject CustomizaitonPrefab;
    public CustomizationPanelScheme HeadItems;
    public CustomizationPanelScheme FaceItems;
    public CustomizationPanelScheme BodyItems;

    [Header("Customization Items")]
    public CustomizationObject HeadItem;
    public CustomizationObject FaceItem;
    public CustomizationObject BodyItem;

    private GameObject previewHead;
    private GameObject previewFace;
    private GameObject previewBody;
    private CustomizationItem previewHeadInfo;
    private CustomizationItem previewFaceInfo;
    private CustomizationItem previewBodyInfo;

    private void Start()
    {
        HeadPlace.SetParent(SpineBone.transform);
        BodyPlace.SetParent(SpineBone.transform);
    }

    public void PreviewItem(CustomizationItem item)
    {
        switch (item.type)
        {
            case CustomizationType.HEAD:
                if (HeadItem.myself.id > -1)
                {
                    HeadItem.gameObject.SetActive(false);
                }

                if (previewHead != null)
                    Destroy(previewHead);

                previewHead = Instantiate(item.item, HeadPlace);
                previewHead.layer = 11;
                previewHeadInfo = item;
                break;
            case CustomizationType.FACE:
                if (FaceItem.myself.id > -1)
                {
                    FaceItem.gameObject.SetActive(false);
                }

                if (previewFace != null)
                    Destroy(previewFace);

                previewFace = Instantiate(item.item, HeadPlace);
                previewFace.layer = 11;
                previewFaceInfo = item;
                break;
            case CustomizationType.BODY:
                if (BodyItem.myself.id > -1)
                {
                    BodyItem.gameObject.SetActive(false);
                }

                if (previewBody != null)
                    Destroy(previewBody);

                previewBody = Instantiate(item.item, BodyPlace);
                previewBody.layer = 11;
                previewBodyInfo = item;
                break;
            default:
                break;
        }

        dirty = true;
    }

    public void EndPreview()
    {
        if(dirty)
        {
            if (previewHead != null)
                Destroy(previewHead);
            if (previewFace != null)
                Destroy(previewFace);
            if (previewBody != null)
                Destroy(previewBody);

            HeadItem.gameObject.SetActive(true);
            BodyItem.gameObject.SetActive(true);
            FaceItem.gameObject.SetActive(true);
        }
        
    }

    public void ConfirmPreview()
    {
        Worker myWorker = GetComponent<Worker>();

        if (previewHead != null)
        {
            previewHead.transform.SetParent(HeadItem.gameObject.transform);
            HeadItem.myself = previewHeadInfo;
            //give happiness to myWorker
        }
        if (previewFace != null)
        {
            previewFace.transform.SetParent(FaceItem.gameObject.transform);
            FaceItem.myself = previewFaceInfo;
        }
        if (previewBody != null)
        {
            previewBody.transform.SetParent(BodyItem.gameObject.transform);
            BodyItem.myself = previewBodyInfo;
        }

        dirty = false;
    }

}

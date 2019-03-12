using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerCustomization : MonoBehaviour
{
    public GameObject SpineBone;

    public Transform HeadPlace;
    public Transform BodyPlace;
    private bool dirty;

    [Header("Customization Scheme")]
    //public GameObject CustomizaitonPrefab;
    [Tooltip("Use Gender Specific List.\n0) Head\n1) Face.\n2) Body.")]
    public ScriptableObjectsList CustomizaitonSchemes;

    public CustomizationPanelScheme HeadItems
    {
        get { return (CustomizationPanelScheme)CustomizaitonSchemes.ListElements[0]; }
    }

    public CustomizationPanelScheme FaceItems
    {
        get { return (CustomizationPanelScheme)CustomizaitonSchemes.ListElements[1]; }
    }

    public CustomizationPanelScheme BodyItems
    {
        get { return (CustomizationPanelScheme)CustomizaitonSchemes.ListElements[2]; }
    }

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
        if (dirty)
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

    //public WorkerCustomizationSerializable GetCustomizationData()
    //{
    //    return new WorkerCustomizationSerializable(HeadItem.myself, FaceItem.myself, BodyItem.myself);
    //}

    public int[] GetCustomizationDataArray()
    {
        int[] arr = new int[3];

        arr[0] = (HeadItem.myself.item != null) ? HeadItem.myself.id : -1;
        arr[1] = (FaceItem.myself.item != null) ? FaceItem.myself.id : -1;
        arr[2] = (BodyItem.myself.item != null) ? BodyItem.myself.id : -1;

        return arr;
    }

    //public void LoadCustomizationData(WorkerCustomizationSerializable cData)
    //{
    //    if (cData.HeadItem.id >= 0)
    //    {
    //        HeadItem.myself.FillData(cData.HeadItem);
    //        var headObj = HeadItems.Items.Find(delegate (CustomizationItem c)
    //        {
    //            if (c.id == HeadItem.myself.id)
    //                return c.item;
    //            else
    //            {
    //                Debug.LogWarning("No Items found with that ID");
    //                return false;
    //            }
    //        });
    //        if (headObj != null)
    //        {
    //            var headPiece = Instantiate(headObj.item, HeadPlace);
    //            HeadItem.myself.item = headPiece;
    //        }
    //    }

    //    if (cData.FaceItem.id >= 0)
    //    {
    //        FaceItem.myself.FillData(cData.FaceItem);
    //        var FaceObj = FaceItems.Items.Find(delegate (CustomizationItem c)
    //        {
    //            if (c.id == FaceItem.myself.id)
    //                return c.item;
    //            else
    //            {
    //                Debug.LogWarning("No Items found with that ID");
    //                return false;
    //            }
    //        });
    //        if (FaceObj != null)
    //        {
    //            var facePiece = Instantiate(FaceObj.item, HeadPlace);
    //            FaceItem.myself.item = facePiece;
    //        }
    //    }

    //    if (cData.BodyItem.id >= 0)
    //    {
    //        BodyItem.myself.FillData(cData.BodyItem);
    //        var BodyObj = BodyItems.Items.Find(delegate (CustomizationItem c)
    //        {
    //            if (c.id == BodyItem.myself.id)
    //                return c.item;
    //            else
    //            {
    //                Debug.LogWarning("No Items found with that ID");
    //                return false;
    //            }
    //        });

    //        if (BodyObj != null)
    //        {
    //            var BodyPiece = Instantiate(BodyObj.item, BodyPlace);
    //            FaceItem.myself.item = BodyPiece;
    //        }
    //    }

    //}

    public void LoadCustomizationData(int[] cData)
    {
        if (cData[0] >= 0)
        {
            //HeadItem.myself.FillData(cData.HeadItem);

            CustomizationItem headItem = HeadItems.Items.Find(delegate (CustomizationItem c)
            {
                if (c.id == cData[0])
                {
                    return c.item;
                }
                else
                {
                    Debug.LogWarning("No Items found with that ID");
                    return false;
                }
            });

            if (headItem.item != null)
            {
                HeadItem.myself = headItem;
                var headPiece = Instantiate(headItem.item, HeadPlace);
                headPiece.layer = 11;
                HeadItem.myself.item = headPiece;
            }
        }

        if (cData[1] >= 0)
        {
            //FaceItem.myself.FillData(cData.FaceItem);
            var faceItem = FaceItems.Items.Find(delegate (CustomizationItem c)
            {
                if (c.id == cData[1])
                {
                    return c.item;
                }
                else
                {
                    Debug.LogWarning("No Items found with that ID");
                    return false;
                }
            });
            if (faceItem != null)
            {
                FaceItem.myself = faceItem;
                var facePiece = Instantiate(faceItem.item, HeadPlace);
                facePiece.layer = 11;
                FaceItem.myself.item = facePiece;
            }
        }

        if (cData[2] >= 0)
        {
            //BodyItem.myself.FillData(cData.BodyItem);

            var BodyObj = BodyItems.Items.Find(delegate (CustomizationItem c)
            {
                if (c.id == cData[2])
                    return c.item;
                else
                {
                    Debug.LogWarning("No Items found with that ID");
                    return false;
                }
            });

            if (BodyObj != null)
            {
                BodyItem.myself = BodyObj;
                var BodyPiece = Instantiate(BodyObj.item, BodyPlace);
                BodyPiece.layer = 11;
                BodyItem.myself.item = BodyPiece;
            }
        }

    }

}

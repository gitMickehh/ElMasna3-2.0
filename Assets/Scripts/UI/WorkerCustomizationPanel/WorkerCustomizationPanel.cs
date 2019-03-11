using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WorkerCustomizationPanel : MonoBehaviour
{
    public GameConfig gameConfigFile;
    public Sprite happyMoneyImage;
    public GameObject storeItem;
    public WorkerField workerSelected;

    [Header("Preview Schemes")]
    public CustomizationPanelScheme headScheme;
    public CustomizationPanelScheme faceScheme;
    public CustomizationPanelScheme bodyScheme;

    private bool filledUp;

    [Header("Camera Preview")]
    public RenderTexture renderImage;
    public GameObject UICameraPrefab;
    private Camera UICamera;

    [Header("UI Elements")]
    public Transform HeadPanel;
    public Transform BodyPanel;
    public Transform FacePanel;
    public Button confirmationButton;

    private List<GameObject> HeadItems = new List<GameObject>();
    private List<GameObject> BodyItems = new List<GameObject>();
    private List<GameObject> FacesItems = new List<GameObject>();

    [Header("Game Manager")]
    [Attributes.GreyOut]
    public GameManager gameManager;

    //Modal Panel
    private ModalPanel modalPanel;
    private UnityAction ConfirmAction;

    private void OnEnable()
    {
        ClearAll();
        confirmationButton.gameObject.SetActive(false);

        updateScheme();
        HeadItems = FillItems(HeadPanel, headScheme.Items);
        FacesItems = FillItems(FacePanel, faceScheme.Items);
        BodyItems = FillItems(BodyPanel, bodyScheme.Items);

        UICamera = Instantiate(UICameraPrefab, new Vector3(), new Quaternion()).GetComponent<Camera>();
        UICamera.targetTexture = renderImage;
        UICamera.transform.SetParent(workerSelected.worker.UICameraPosition);
        UICamera.transform.localPosition = new Vector3();
        UICamera.transform.rotation = new Quaternion();
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        modalPanel = ModalPanel.Instance();

        ConfirmAction = new UnityAction(ConfirmPreviews);

    }

    private List<GameObject> FillItems(Transform panel, List<CustomizationItem> items)
    {

        List<GameObject> listOfObjectsCreated = new List<GameObject>();

        for (int i = 0; i < items.Count; i++)
        {
            var item = Instantiate(storeItem, panel);
            var uiItem = item.GetComponent<UICustomizationObject>();

            uiItem.FillInObject(items[i], this);

            listOfObjectsCreated.Add(item);
        }

        return listOfObjectsCreated;
    }

    private void ClearAll()
    {
        for (int i = 0; i < HeadItems.Count; i++)
        {
            Destroy(HeadItems[i]);
        }

        for (int i = 0; i < BodyItems.Count; i++)
        {
            Destroy(BodyItems[i]);
        }

        for (int i = 0; i < FacesItems.Count; i++)
        {
            Destroy(FacesItems[i]);
        }

        HeadItems.Clear();
        FacesItems.Clear();
        BodyItems.Clear();
    }

    private void updateScheme()
    {
        var c = workerSelected.worker.customization;

        headScheme = c.HeadItems;
        faceScheme = c.FaceItems;
        bodyScheme = c.BodyItems;
    }

    private void WorkerScheme(CustomizationPanelScheme h, CustomizationPanelScheme f, CustomizationPanelScheme b)
    {
        headScheme = h;
        faceScheme = f;
        bodyScheme = b;
    }

    public void ConfirmButton()
    {
        string s = gameConfigFile.CurrentLanguageProfile.AreYouSure + gameConfigFile.CurrentLanguageProfile.QuestionMark;
        modalPanel.Choice(s,ConfirmAction);
    }

    private void ConfirmPreviews()
    {
        workerSelected.worker.customization.ConfirmPreview();
        //pay here
    }

    public void CancelPreview()
    {
        workerSelected.worker.customization.EndPreview();
    }

    public void OnBackButtonPressed()
    {
        if (HeadPanel.gameObject.activeSelf || BodyPanel.gameObject.activeSelf || FacePanel.gameObject.activeSelf)
        {
            HeadPanel.gameObject.SetActive(false);
            BodyPanel.gameObject.SetActive(false);
            FacePanel.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetDirty(bool dirty)
    {
        confirmationButton.gameObject.SetActive(dirty);
    }

}

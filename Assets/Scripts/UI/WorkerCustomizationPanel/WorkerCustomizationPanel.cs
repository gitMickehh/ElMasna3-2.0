using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WorkerCustomizationPanel : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public GameObjectField gameManager;
    public GameConfig gameConfigFile;
    public WorkerField workerSelected;
    public ColorsList ListOfColors;
    public ColorField CurrentUniformColor;

    [Header("Prefabs")]
    public GameObject storeItem;
    public GameObject UICameraPrefab;
    public GameObject colorButtonPrefab;
    public Material WorkersUniformMaterial;

    [Header("Preview Schemes")]
    [Attributes.GreyOut]
    public CustomizationPanelScheme headScheme;
    [Attributes.GreyOut]
    public CustomizationPanelScheme faceScheme;
    [Attributes.GreyOut]
    public CustomizationPanelScheme bodyScheme;

    private bool filledUp;

    [Header("Camera Preview")]
    public RenderTexture renderImage;
    private Camera UICamera;

    [Header("UI Elements")]
    public Sprite happyMoneyImage;
    public Transform HeadPanel;
    public Transform BodyPanel;
    public Transform FacePanel;
    public Transform ColorPanel;
    public Button confirmationButton;

    private List<GameObject> HeadItems = new List<GameObject>();
    private List<GameObject> BodyItems = new List<GameObject>();
    private List<GameObject> FaceItems = new List<GameObject>();
    private List<GameObject> ColorsButtons = new List<GameObject>();

    //Modal Panel
    private ModalPanel modalPanel;
    private UnityAction ConfirmAction;

    private float totalCost;


    private void OnEnable()
    {
        ClearAll();
        confirmationButton.gameObject.SetActive(false);

        updateScheme();

        HeadItems = FillItems(HeadPanel, headScheme.Items);
        FaceItems = FillItems(FacePanel, faceScheme.Items);
        BodyItems = FillItems(BodyPanel, bodyScheme.Items);
        FillColorButtons();


        UICamera = Instantiate(UICameraPrefab, new Vector3(), new Quaternion()).GetComponent<Camera>();
        UICamera.targetTexture = renderImage;
        UICamera.transform.SetParent(workerSelected.worker.UICameraPosition);
        UICamera.transform.localPosition = new Vector3();
        UICamera.transform.rotation = new Quaternion();
    }

    private void Start()
    {
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

        for (int i = 0; i < FaceItems.Count; i++)
        {
            Destroy(FaceItems[i]);
        }

        for (int i = 0; i < ColorsButtons.Count; i++)
        {
            Destroy(ColorsButtons[i]);
        }

        HeadItems.Clear();
        FaceItems.Clear();
        BodyItems.Clear();
        ColorsButtons.Clear();
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
        //totalCost = 
        string s = gameConfigFile.CurrentLanguageProfile.AreYouSure + gameConfigFile.CurrentLanguageProfile.QuestionMark;
        modalPanel.Choice(s, ConfirmAction);
    }

    private void ConfirmPreviews()
    {
        workerSelected.worker.customization.ConfirmPreview();
        UniformChanged();

        CloseAllPanels();

        //pay here
        //withdraw totalCost
    }

    public void CancelPreview()
    {
        workerSelected.worker.customization.EndPreview();
    }

    public void OnBackButtonPressed()
    {
        if (HeadPanel.gameObject.activeSelf || BodyPanel.gameObject.activeSelf || FacePanel.gameObject.activeSelf || ColorPanel.gameObject.activeSelf)
        {
            HeadPanel.gameObject.SetActive(false);
            BodyPanel.gameObject.SetActive(false);
            FacePanel.gameObject.SetActive(false);
            ColorPanel.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void CloseAllPanels()
    {
        HeadPanel.gameObject.SetActive(false);
        BodyPanel.gameObject.SetActive(false);
        FacePanel.gameObject.SetActive(false);
        ColorPanel.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void SetDirty(bool dirty)
    {
        confirmationButton.gameObject.SetActive(dirty);
    }

    public void FillColorButtons()
    {
        for (int i = 0; i < ListOfColors.Count; i++)
        {
            var c = Instantiate(colorButtonPrefab, ColorPanel);
            c.GetComponent<Image>().color = ListOfColors.colors[i];

            ColorsButtons.Add(c);
        }
    }

    private void UniformChanged()
    {
        CurrentUniformColor.SetValue(WorkersUniformMaterial.color);
    }
}

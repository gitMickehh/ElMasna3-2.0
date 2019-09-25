using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_GuideBook : MonoBehaviour
{
    public GameConfig configFile;
    //public GuideBook book;
    private GuideBook book;
    public GameEvent buttonClickEvent;

    private GuideBookTab currentTab;
    private int activeTab;

    public Text tabTitle;
    public List<Button> Tabs;
    public List<Button> Pages;

    [Space]
    [Header("UI Elements")]
    public Text pageText;
    public Image pageTextReplacer;
    public Image pageImage;
    public Button play_VO_Button;

    private UnityAction playVOButtonEvent;
    private int selectedVOPage;

    private void OnEnable()
    {
        book = configFile.CurrentLanguageProfile.Manual;
        activeTab = 0;
        FillPageTitles(book.tabs[activeTab]);
        playVOButtonEvent = new UnityAction(PlayClipFromSelected);
    }

    //private void Start()
    //{
    //    //StartCoroutine(TestFillPage());
    //    activeTab = 0;
    //    FillPageTitles(book.tabs[activeTab]);
    //}

    //private IEnumerator TestFillPage()
    //{
    //    int i = 0;
    //    while (true)
    //    {
    //        FillPageTitles(book.tabs[i]);
    //        yield return new WaitForSeconds(4);
    //        i = (i + 1) % book.tabs.Length;
    //    }
    //}

    public void ActivateTab(int tabNumber)
    {
        activeTab = tabNumber;
        FillPageTitles(book.tabs[tabNumber]);
    }

    private void FillButtonListener(Button b, int buttonNumber)
    {
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(buttonClickEvent.Raise);
        b.onClick.AddListener(() => FillPage(buttonNumber));
    }

    public void FillPageTitles(GuideBookTab currentTab)
    {
        this.currentTab = currentTab;
        tabTitle.text = currentTab.name;
        
        for (int i = 0; i < currentTab.listOfPages.Length; i++)
        {
            //fill in page titles
            Pages[i].gameObject.SetActive(true);
            Pages[i].GetComponentInChildren<Text>().text = currentTab.listOfPages[i].PageTitle;
            FillButtonListener(Pages[i],i);
        }
        for (int i = currentTab.listOfPages.Length; i < Pages.Count; i++)
        {
            Pages[i].gameObject.SetActive(false);
        }

        FillPage(0);
    }

    public void FillPage(int pageNumberInTab)
    {
        //This function will be added to the listener of each button responding to a page

        if (currentTab.listOfPages[pageNumberInTab].PageImage != null)
            pageImage.sprite = currentTab.listOfPages[pageNumberInTab].PageImage;

        if(currentTab.listOfPages[pageNumberInTab].ImageDescription)
        {
            pageTextReplacer.gameObject.SetActive(true);
            pageText.gameObject.SetActive(false);

            pageTextReplacer.sprite = currentTab.listOfPages[pageNumberInTab].DescriptionTextImage;
        }
        else
        {
            pageText.gameObject.SetActive(true);
            pageTextReplacer.gameObject.SetActive(false);

            pageText.text = currentTab.listOfPages[pageNumberInTab].PageDescription;
        }

        if(currentTab.listOfPages[pageNumberInTab].VO_Clip != null)
        {
            //if it has a voice over clip
            play_VO_Button.gameObject.SetActive(true);
            play_VO_Button.onClick.RemoveAllListeners();

            selectedVOPage = pageNumberInTab;
            play_VO_Button.onClick.AddListener(playVOButtonEvent);
        }
        else
        {
            play_VO_Button.gameObject.SetActive(false);
        }

        SetSelectedButtonInactive(pageNumberInTab);
    }

    public void PlayClipFromSelected()
    {
        AudioManager.instance.PlaySound(currentTab.listOfPages[selectedVOPage].VO_Clip);
    }

    private void SetSelectedButtonInactive(int buttonNumber)
    {
        //TODO: make the button selected actiated and the rest not.
        for (int i = 0; i < Tabs.Count; i++)
        {
            Tabs[i].interactable = true;
        }
        Tabs[activeTab].interactable = false;

        for (int i = 0; i < Pages.Count; i++)
        {
            Pages[i].interactable = true;
        }

        Pages[buttonNumber].interactable = false;
    }



}

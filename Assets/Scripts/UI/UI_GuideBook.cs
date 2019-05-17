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

    public Text pageText;
    public Image pageImage;

    private void Awake()
    {
        book = configFile.CurrentLanguageProfile.Manual;
    }

    private void Start()
    {
        //StartCoroutine(TestFillPage());
        activeTab = 0;
        FillPageTitles(book.tabs[activeTab]);
    }

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

        pageText.text = currentTab.listOfPages[pageNumberInTab].PageDescription;
        SetSelectedButtonInactive(pageNumberInTab);
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

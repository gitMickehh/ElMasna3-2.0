using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsPanel : MonoBehaviour
{
    public List<GameObject> creditPanels;

    public Button nextPanelBtn;
    public Button backPanelBtn;

    int currentPanel;
    
    private void OnEnable()
    {
        backPanelBtn.interactable = false;
        nextPanelBtn.interactable = true;
        currentPanel = 0;
        creditPanels[currentPanel].SetActive(true);
    }

    private bool CanGoNext()
    {
        return currentPanel < creditPanels.Count-1;
    }

    private bool CanGoBack()
    {
        return currentPanel > 0;
    }

    public void GoNext()
    {
        if (CanGoNext())
        {
            creditPanels[currentPanel].SetActive(false);
            currentPanel++;
            creditPanels[currentPanel].SetActive(true);

            if(!CanGoNext())
                nextPanelBtn.interactable = false;

            if (CanGoBack())
                backPanelBtn.interactable = true;
        }

    }

    public void GoBack()
    {
        if (CanGoBack())
        {
            creditPanels[currentPanel].SetActive(false);
            currentPanel--;
            creditPanels[currentPanel].SetActive(true);

            if(!CanGoBack())
                backPanelBtn.interactable = false;

            if (CanGoNext())
                nextPanelBtn.interactable = true;

        }

    }
}

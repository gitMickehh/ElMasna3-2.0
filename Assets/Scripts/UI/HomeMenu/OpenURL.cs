using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public StringField naderURL;
    public StringField safwaURL;
    public StringField asmaaURL;
    public StringField marwaURL;
    public StringField noreenURL;

    Dictionary<string, string> teamURL;
    
    void Start()
    {
        teamURL = new Dictionary<string, string>()
        {
            { "Nader", naderURL.value },
            { "Safwa", safwaURL.value },
            { "Asmaa", asmaaURL.value },
            { "Marwa", marwaURL.value },
            { "Noreen", noreenURL.value }

        };
    }

    public void OpenNameURL(string name)
    {
        Application.OpenURL(teamURL[name]);
    }
}

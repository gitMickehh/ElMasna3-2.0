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
    public StringField kareemURL;
    public StringField itiURL;
    public StringField gizURL;
    public StringField volkerURL;
    public StringField frankURL;

    Dictionary<string, string> teamURL;
    
    void Start()
    {
        teamURL = new Dictionary<string, string>()
        {
            { "Nader", naderURL.value },
            { "Safwa", safwaURL.value },
            { "Asmaa", asmaaURL.value },
            { "Marwa", marwaURL.value },
            { "Noreen", noreenURL.value },
            { "Kareem", kareemURL.value },
            { "ITI", itiURL.value },
            { "GIZ", gizURL.value },
            { "Volker", volkerURL.value },
            { "Frank", frankURL.value },

        };
    }

    public void OpenNameURL(string name)
    {
        Application.OpenURL("http://"+teamURL[name]);
    }
}

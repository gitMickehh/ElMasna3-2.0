using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Worker))]
public class WorkerEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        Worker workerC = (Worker)target;
        if(GUILayout.Button("Randomize Worker"))
        {
            workerC.RandomizeWorker();
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveGameManager))]
public class SaveManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        SaveGameManager saveManager = (SaveGameManager)target;

        if (GUILayout.Button("Reset Save"))
        {
            saveManager.ClearSave();
        }

    }
}

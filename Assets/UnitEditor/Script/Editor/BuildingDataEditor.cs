using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuildingData))]
public class BuildingDataEditor : Editor
{
    private new SerializedProperty name;
    private SerializedProperty type;
    private SerializedProperty tooltip;
    private SerializedProperty buildTime;
    private SerializedProperty deathTime;


    private void OnEnable()
    {
        SetProperties();
    }

    private void SetProperties()
    {
        name = serializedObject.FindProperty("name");
        type = serializedObject.FindProperty("type");
        tooltip = serializedObject.FindProperty("tooltip");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.DrawDefaultInspector();
       
        //if (name != null) EditorGUI.PropertyField(new Rect(0f, 10f, 300f, 65f), name);
        //if (name != null) EditorGUI.PropertyField(new Rect(0f, 80f, 300f, 65f), type);

        serializedObject.ApplyModifiedProperties();
    }

}

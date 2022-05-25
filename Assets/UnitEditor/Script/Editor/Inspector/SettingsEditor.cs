/// <author> Thomas Krahl </author>

using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using UnitEditor.Data;
using UnitEditor.UI.Section;
using UnitEditor.UI.Custom;


[CustomEditor(typeof(UnitEditorData))]
public class SettingsEditor : Editor
{
    #region PrivateFields

    private GUISkin mySkin;
    private ReorderableList prefabList;

    private SerializedProperty resourcesPath;
    private SerializedProperty unitFolderName;
    private SerializedProperty unitPrefabs;

    #endregion

    #region UnityFunctions

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDestroy()
    {
        Destroy();
    }

    #endregion

    #region GUI

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //base.DrawDefaultInspector();

        EditorGUILayout.Space(5f);
        EditorGUILayout.BeginHorizontal(GUILayout.Width(390f));

        EditorGUILayout.Space(10f);
        EditorGUILayout.BeginVertical(GUILayout.Height(190f));

        EditorGUILayout.PropertyField(resourcesPath, GUILayout.Width(300f));
        EditorGUILayout.Space(2f);

        EditorGUILayout.PropertyField(unitFolderName, GUILayout.Width(300f));
        EditorGUILayout.Space(10f);

        prefabList.DoLayoutList();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }



    #endregion

    #region Setup

    private void Initialize()
    {
        mySkin = DataHandler.Instance.MySkin;

        SetProperties();
        InitList();
    }

    private void SetProperties()
    {
        resourcesPath = serializedObject.FindProperty("resourcesPath");
        unitFolderName = serializedObject.FindProperty("unitsRootFolderName");
        unitPrefabs = serializedObject.FindProperty("unitTemplates");
    }

    private void InitList()
    {
        prefabList = new ReorderableList(unitPrefabs.serializedObject, unitPrefabs, false, true, true, true);
        prefabList.drawHeaderCallback += DrawListHeader;
        prefabList.drawElementCallback += DrawListElements;
    }

    #endregion

    #region Destroy

    protected virtual void Destroy()
    {

    }

    #endregion

    #region List

    private void DrawListHeader(Rect headerRect)
    {
        EditorGUI.LabelField(headerRect, "UnitTypePrefabs", mySkin.FindStyle("sliderLabelGrey"));
    }

    private void DrawListElements(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = prefabList.serializedProperty.GetArrayElementAtIndex(index);

        string unitType = string.Empty;
        if (index > Enum.GetValues(typeof(UnitType)).Length - 1)
        {
            unitType = "Undefined Type";
        }
        else
        {
            unitType = ((UnitType)index).ToString() + " Prefab";
        }
        
        EditorGUI.LabelField(new Rect(rect.x, rect.y, 150f, rect.height), unitType);
        EditorGUI.PropertyField(new Rect(rect.x + 155f, rect.y, 200f, rect.height), element, GUIContent.none);
    }

    #endregion
}


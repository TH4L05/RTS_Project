using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "NewEditorData", menuName = "Data/EditorData")]
public class UnitEditorData : ScriptableObject
{
    public string resourcesPath = "Assets/";
    public string unitsRootFolderName = "Units";

    public GameObject[] unitTemplates = new GameObject[Enum.GetValues(typeof(UnitType)).Length - 1];
}

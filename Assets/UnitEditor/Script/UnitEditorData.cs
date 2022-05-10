using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewEditorData", menuName = "Data/EditorData")]
public class UnitEditorData : ScriptableObject
{
    public string resourcesPath = "Assets/";
    public string unitsRootFolderName = "Units";

    public Texture healthIcon;
    public Texture manaIcon;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewEditorData", menuName = "Data/EditorData")]
public class UnitEditorData : ScriptableObject
{
    public string unitsRootPath = "Assets/Templates/";
    public string unitsBuildingsFolderName = "Buildings";
    public string unitsCharactersFolderName = "Characters";
    public List<GameObject> buildings = new List<GameObject>();
    public List<GameObject> characters = new List<GameObject>();
}

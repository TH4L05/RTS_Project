using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "NewEditorData", menuName = "Data/EditorData")]
public class UnitEditorData : ScriptableObject
{
    public string resourcesPath = "Assets/";
    public string unitsRootFolderName = "Units";

    public GameObject buildingTemplate;
    public GameObject characterTemplate;

    public GameObject[] unitTemplates = new GameObject[Enum.GetValues(typeof(UnitType)).Length - 1];

    [SerializeField]
    private ResourceSetup[] suppliedResourcesOnStart = new ResourceSetup[]
                                                                {new ResourceSetup(ResourceType.Wood, 0),
                                                                 new ResourceSetup(ResourceType.Gold, 0),
                                                                 new ResourceSetup(ResourceType.Food, 0),
                                                                 new ResourceSetup(ResourceType.Unit, 0)
                                                                };
}

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewBuildingData", menuName = "Data/BuildingData")]
public class BuildingData : UnitData
{
    #region SerializedFields

    [Header("Building")]
    [SerializeField] private ResourceSetup[] producedResources = new ResourceSetup[]
                                                                                    {
                                                                                    new ResourceSetup(ResourceType.Wood, 0),
                                                                                    new ResourceSetup(ResourceType.Gold, 0),
                                                                                    new ResourceSetup(ResourceType.Food, 0),
                                                                                    new ResourceSetup(ResourceType.Unit, 0)
                                                                                    };

    [SerializeField] private ResourceSetup[] suppliedResourcesOnStart = new ResourceSetup[]
                                                                                    {
                                                                                    new ResourceSetup(ResourceType.Wood, 0),
                                                                                    new ResourceSetup(ResourceType.Gold, 0),
                                                                                    new ResourceSetup(ResourceType.Food, 0),
                                                                                    new ResourceSetup(ResourceType.Unit, 0)
                                                                                    };
    [SerializeField] private float productionSpeed = 5f;

    #endregion

    #region PublicFields

    public ResourceSetup[] ProducedResources => producedResources;
    public float ProductionSpeed => productionSpeed;
    public ResourceSetup[] SuppliedResourcesOnStart => suppliedResourcesOnStart;

    #endregion

    public override void SetDataFromStrings(string[] data)
    {
        base.SetDataFromStrings(data);

        producedResources[0].amount = int.Parse(data[37]);
        producedResources[1].amount = int.Parse(data[38]);
        producedResources[2].amount = int.Parse(data[39]);
        producedResources[3].amount = int.Parse(data[40]);

        productionSpeed = float.Parse(data[41]);

        suppliedResourcesOnStart[0].amount = int.Parse(data[42]);
        suppliedResourcesOnStart[1].amount = int.Parse(data[43]);
        suppliedResourcesOnStart[2].amount = int.Parse(data[44]);
        suppliedResourcesOnStart[3].amount = int.Parse(data[45]);
    }
}

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

}

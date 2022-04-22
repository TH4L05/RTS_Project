using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BuildingType
{
    Undefined = -1,
    ResourceProduction,
    CharacterProduction,
}

[CreateAssetMenu(fileName = "NewBuildingData", menuName = "Data/BuildingData")]
public class BuildingData : UnitData
{
    #region SerializedFields

    [Header("Building")]
    [SerializeField] private BuildingType buildingType = BuildingType.Undefined;
    [SerializeField] private ResourceType producedResource;
    [SerializeField] private int productionAmount = 1;
    [SerializeField] private float productionSpeed = 1f;
    [SerializeField] private ResourceSetup[] provideResourcesOnBuild;

    #endregion

    #region PublicFields

    public BuildingType BuildingType => buildingType;
    public ResourceType ProducedResource => producedResource;
    public int ProductionAmount => productionAmount;
    public float ProductionSpeed => productionSpeed;
    public ResourceSetup[] ProvideResourcesOnBuild => provideResourcesOnBuild;

    #endregion

}

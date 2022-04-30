using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewBuildingData", menuName = "Data/BuildingData")]
public class BuildingData : UnitData
{
    #region SerializedFields

    [Header("Building")]
    [SerializeField] private ResourceSetup[] producedResources;
    [SerializeField] private float productionSpeed = 1f;
    [SerializeField] private ResourceSetup[] suppliedResourcesOnStart;

    #endregion

    #region PublicFields

    public ResourceSetup[] ProducedResources => producedResources;
    public float ProductionSpeed => productionSpeed;
    public ResourceSetup[] SuppliedResourcesOnStart => suppliedResourcesOnStart;

    #endregion

}

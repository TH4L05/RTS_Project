

using UnityEngine;

public class Building : Unit
{
    #region SerializedFields

    [SerializeField] private BuildingData data;
    [SerializeField] private Transform spawn;
    


    #endregion

    #region PublicFields

    public BuildingData Data => data;
    public Transform Spawn => spawn;

    #endregion

    #region Setup

    protected override void StartSetup()
    {
        base.StartSetup();
        currentHealth = data.HealthMax;

        if (data.ProvideResourcesOnBuild == null) return;
        ProvideResources();
    }

    protected override void AdditionalSetup()
    {
        base.AdditionalSetup();
        if (data.BuildingType == BuildingType.ResourceProduction)
        {
            InvokeRepeating("ResourceProduced", 1, data.ProductionSpeed);
        }
    }

    protected override void SetUnitType()
    {
        unitType = data.UType;
    }

    #endregion

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (healthBar != null) healthBar.UpdateValue(currentHealth, data.HealthMax);
    }


    private void ProvideResources()
    {
        foreach (var resource in data.ProvideResourcesOnBuild)
        {
            if (humanConrolledUnit)
            {
                ResourceManager.GainResource?.Invoke(owner, resource.ResoureData.Type, resource.amount, true);
            }
            else
            {
                ResourceManager.GainResource?.Invoke(owner, resource.ResoureData.Type, resource.amount, false);
            }
        }
    }


    private void ResourceProduced()
    {
        //Debug.Log($"Resource Produced: {data.ProducedResource}");
        if (humanConrolledUnit)
        {
            ResourceManager.GainResource?.Invoke(owner, data.ProducedResource, data.ProductionAmount, true);
        }
        else
        {
            ResourceManager.GainResource?.Invoke(owner, data.ProducedResource, data.ProductionAmount, false);
        }       
    }

    public void SetSpawnPos(Vector3 pos)
    {
        spawn.position = pos;
    }

}

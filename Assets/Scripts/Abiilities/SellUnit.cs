/// <author> Thomas Krahl </author>

using UnityEngine;

[CreateAssetMenu(fileName = "SellUnitAbility", menuName = "Data/Abilities/SellUnitAbility")]
public class SellUnit : Ability
{
    #region Actions



	#endregion

	#region SerializedFields



	#endregion

	#region PrivateFields



	#endregion

	#region PublicFields



	#endregion

	#region UnityFunctions

	void Awake()
	{
		
	}
	

	void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    #endregion

    public override void DoAction(GameObject obj)
    {
        base.DoAction(obj);
        var unit = obj.GetComponent<Unit>();

        foreach (var resource in unit.UnitData.RequiredResources)
        {
            ResourceManager.GainResource(unit.Owner, resource.resourceType, resource.amount / 2, true);
        }

        Game.Instance.PlayerManager.RemoveUnit(unit, unit.Owner);
        Game.Instance.Unitselection.DeselectUnits();
        Destroy(obj);
    }
}

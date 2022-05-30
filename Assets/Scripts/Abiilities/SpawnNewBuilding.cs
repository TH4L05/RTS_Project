/// <author> Thomas Krahl </author>

using UnityEngine;


[CreateAssetMenu(fileName = "SpawnNewUnitBuildingAbility", menuName = "Data/Abilities/SpawnNewBuilingAbility")]
public class SpawnNewBuilding : Ability
{
    public override void DoAction(GameObject obj)
    {
        base.DoAction(obj);
        Game.Instance.BuildMode.ActivateMode(unitTemplate);
    }
}

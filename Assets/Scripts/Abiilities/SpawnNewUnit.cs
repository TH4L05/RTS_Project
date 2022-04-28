using UnityEngine;

[CreateAssetMenu(fileName = "SpawnNewUnitAbility", menuName = "Data/Abilities/SpawnNewUnitAbility")]
public class SpawnNewUnit : Ability
{
    public override void DoAction(GameObject obj)
    {
        base.DoAction(obj);
        Building building = obj.GetComponent<Building>();
        if (building == null) return;
        building.BuildNewUnit(unitTemplate);

    }
}

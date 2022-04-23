using UnityEngine;

[CreateAssetMenu(fileName = "SpawnNewUnitAbility", menuName = "Data/Abilities/SpawnNewUnitAbility")]
public class SpawnNewUnit : Ability
{
    public override void DoAction(GameObject obj)
    {
        base.DoAction(obj);
        var building = obj.GetComponent<Building>();
        if (building == null) return;
        var newUnit = Instantiate(unitTemplate, building.Spawn.position, Quaternion.identity);
        Game.Instance.PlayerManager.AddUnit(newUnit.GetComponent<Unit>(), PlayerType.Human);
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "SpawnNewUnitAbility", menuName = "Data/Abilities/SpawnNewUnitAbility")]
public class SpawnNewUnit : Ability
{
    [SerializeField] protected GameObject unit;



    public override void DoAction()
    {
        var building = obj.GetComponent<Building>();
        if (building == null) return;
        var newUnit = Instantiate(unit, building.Spawn.position, Quaternion.identity);
        Game.Instance.PlayerManager.AddUnit(newUnit.GetComponent<Unit>(), PlayerType.Human);
    }
}

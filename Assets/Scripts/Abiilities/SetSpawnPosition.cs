using UnityEngine;

[CreateAssetMenu(fileName = "SetSpawnPointAbility", menuName = "Data/Abilities/SetSpawnPointAbility")]
public class SetSpawnPosition : Ability
{
    public override void DoAction(GameObject obj)
    {
        base.DoAction(obj);
        var building = obj.GetComponent<Building>();
        if (building == null) return;
        Vector3 position = Game.Instance.SelectionHandler.RaycastHitPoint;
        building.SetSpawnPos(position);
    }
}

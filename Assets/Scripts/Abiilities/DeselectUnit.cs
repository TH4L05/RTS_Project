using UnityEngine;

[CreateAssetMenu(fileName = "DeselectUnitAbility", menuName = "Data/Abilities/DeselectUnitAbility")]
public class DeselectUnit : Ability
{
    public override void DoAction(GameObject obj)
    {
        base.DoAction(obj);
        Game.Instance.SelectionHandler.DeselectUnit();
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "DeselectUnitAbility", menuName = "Data/Abilities/DeselectUnitAbility")]
public class DeselectUnit : Ability
{
    public override void DoAction()
    {
        Game.Instance.SelectionHandler.DeselectUnit();
    }
}

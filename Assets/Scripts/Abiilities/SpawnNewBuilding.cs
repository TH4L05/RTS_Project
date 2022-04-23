using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SpawnNewUnitBuildingAbility", menuName = "Data/Abilities/SpawnNewBuilingAbility")]
public class SpawnNewBuilding : Ability
{
    [SerializeField] private GameObject UnitTemplate;

    public override void DoAction()
    {
        Game.Instance.BuildMode.ActivateMode(UnitTemplate);
    }

    public override void SetObject(GameObject obj)
    {
    }
}

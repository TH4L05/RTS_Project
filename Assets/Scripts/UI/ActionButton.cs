using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class ActionData
{
    public ActionType ActionType;
}

public enum ActionType
{
    Default,
    CharacterProduction,
}

public class ActionButton: MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image icon;
    private int index;

    public void SetAction(BuildingData data, int gridIndex)
    {
        SetBuildingAction(data, gridIndex);
    }

    public void SetAction(CharacterData data, int gridIndex)
    {
        
    }

    private void SetBuildingAction(BuildingData data, int idx)
    {
        index = idx;

        switch (data.BuildingType)
        {
            case BuildingType.Undefined:
            default:
                return;


            case BuildingType.ResourceProduction:
                return;


            case BuildingType.CharacterProduction:
                button.interactable = true;             
                button.onClick.AddListener(SpawnCharacter);
                SetIconBuilding(data);
                break;
        }
    }

    private void SetIconBuilding(BuildingData data)
    {
        var sprite = data.ProducedUnits[index].gameObject.GetComponent<Character>().Data.ActionButtonIcon;
        Debug.Log(sprite);
        if (sprite == null) return;
        icon.sprite = sprite;
    }

    public void RemoveAction()
    {
        button.onClick.RemoveAllListeners();
        button.interactable = false;
        icon.sprite = null;
        index = -1;
    }

    #region ButtonActions

    public void SpawnCharacter()
    {
        Debug.Log("SpawnACharacter");
        var activeObj = Game.Instance.GetSelectedObj();
        var building = activeObj.GetComponent<Building>();
        var data = building.Data;

        var character = data.ProducedUnits[index];

        Instantiate(character, building.Spawn.position, Quaternion.identity);

    }

    public void Deselect()
    {

    }

    #endregion
}

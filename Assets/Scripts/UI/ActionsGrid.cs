using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsGrid : MonoBehaviour
{
    public List<ActionButton> actionButtons = new List<ActionButton>();

    private void Start()
    {
        SelectionHandler.ObjectSelected += UpdateButtonsOnSelection;
        SelectionHandler.ObjectDeselected += UpdateButtonsOnDeselection;
    }

    public void UpdateButtonsOnSelection(Unit unit)
    {
        UpdateButtonsOnDeselection();

        if (unit == null) return;

        /*var tag = obj.tag;

        switch (tag)
        {
            case "Building":
                BuildingActions(obj);
                break;

            case "Character":
                CharacterActions(obj);
                break;

            default:
                break;
        }*/

    }

    private void BuildingActions(GameObject obj)
    {
        var data = obj.GetComponent<Building>().Data;
        var amount = data.ProducedUnits.Length;

        if (amount == 0) return;

        for (int i = 0; i < amount; i++)
        {
            actionButtons[i].SetAction(data, i);
        }
    }


    private void CharacterActions(GameObject obj)
    {
        var data = obj.GetComponent<Character>().Data;
        var amount = data.ProducedUnits.Length;

        if (amount == 0) return;

        for (int i = 0; i < amount; i++)
        {
            actionButtons[i].SetAction(data, i);
        }
    }

    public void UpdateButtonsOnDeselection()
    {
        foreach (var button in actionButtons)
        {           
            button.RemoveAction();
        }
    }
}

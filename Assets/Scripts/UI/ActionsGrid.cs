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

    private void OnDestroy()
    {
        SelectionHandler.ObjectSelected -= UpdateButtonsOnSelection;
        SelectionHandler.ObjectDeselected -= UpdateButtonsOnDeselection;
    }

    public void UpdateButtonsOnSelection(Unit unit)
    {
        UpdateButtonsOnDeselection();

        if (unit == null) return;
        if (Game.Instance.HumanContolledUnit(unit))
        {
            Debug.Log("HUMAN UNIT");
            var index = 0;
            foreach (var actionButton in actionButtons)
            {
                actionButton.SetAction(unit, index);
                index++;
            }
        }     
        else
        {
            Debug.Log("Computer UNIT");
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

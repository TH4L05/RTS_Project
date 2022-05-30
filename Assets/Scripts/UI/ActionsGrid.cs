/// <author> Thomas Krahl </author>

using System.Collections.Generic;
using UnityEngine;

public class ActionsGrid : MonoBehaviour
{
    public List<ActionButton> actionButtons = new List<ActionButton>();

    private void Start()
    {
        UnitSelection.ObjectSelected += UpdateButtonsOnSelection;
        UnitSelection.ObjectDeselected += UpdateButtonsOnDeselection;
    }

    private void OnDestroy()
    {
        UnitSelection.ObjectSelected -= UpdateButtonsOnSelection;
        UnitSelection.ObjectDeselected -= UpdateButtonsOnDeselection;
    }

    public void UpdateButtonsOnSelection(GameObject obj)
    {
        UpdateButtonsOnDeselection();

        if (obj == null) return;
        var unit = obj.GetComponent<Unit>();
        if (unit.HumanControlledUnit)
        {
            var index = 0;
            foreach (var actionButton in actionButtons)
            {
                actionButton.SetAction(obj, index);
                index++;
            }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildQueueBar : MonoBehaviour
{
    [SerializeField] private List<QueueButton> queueButtons = new List<QueueButton>();

    private void Start()
    {
        SelectionHandler.ObjectSelected += OnSelection;
        SelectionHandler.ObjectDeselected += OnDeselection;
    }

    private void OnDestroy()
    {
        SelectionHandler.ObjectSelected -= OnSelection;
        SelectionHandler.ObjectDeselected -= OnDeselection;
    }


    private void OnSelection(GameObject obj)
    {
        var unit = obj.GetComponent<Unit>();
        if (unit.UnitType != UnitType.Building) return;
        var building = obj.GetComponent<Building>();
        UpdateQueue(building.buildQueue);


    }

    private void OnDeselection()
    {
        foreach (var button in queueButtons)
        {
            button.ChangeVisibility(false);
        }
    }

    private void UpdateQueue(Queue<GameObject> buildQueue)
    {

        int index = 0;
        foreach (var button in queueButtons)
        {
            button.ChangeVisibility(false);
        }

        index = 0;
        foreach (var item in buildQueue)
        {
            queueButtons[index].ChangeVisibility(true);
            index++;
        }
    }
}

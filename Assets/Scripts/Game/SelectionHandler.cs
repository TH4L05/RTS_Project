using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionHandler : MonoBehaviour
{
    #region Actions

    public static Action<GameObject> ObjectSelected;
    public static Action ObjectDeselected;

    #endregion

    #region Fields

    private Camera cam;
    private List<Unit> selectedUnits = new List<Unit>();
    private RaycastHit hit;
    private Unit hoveredUnit;
    private bool pause;

    public Unit ActiveUnit => selectedUnits[0];
    public Vector3 RaycastHitPoint => hit.point;

    #endregion

    #region UnityFunctions

    private void Awake()
    {
        cam = Camera.main;
        Unit.UnitIsDead += DeselectIfUnitIsDead;
    }

    private void OnDestroy()
    {
        Unit.UnitIsDead -= DeselectIfUnitIsDead;
    }

    void Update()
    {
        if (pause) return;
        MousePosRayCast();
        ShowHealthbar();
        CheckLeftMouseClick();
        CheckRightMouseClick();
    }

    #endregion

    private void MousePosRayCast()
    {
        Ray ray = cam.ScreenPointToRay(Game.Instance.GetMousePosition());
        Physics.Raycast(ray, out hit, 999f);
        //Debug.Log(hit.transform.gameObject.name);
    }

    #region MouseClick

    void CheckLeftMouseClick()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !BuildMode.IsActive)
        {
            var selectable = hit.transform.GetComponent<Unit>();
            
            if (selectable)
            {
                DeselectUnit();
                SelectUnit(selectable);
            }
            else if(selectedUnits.Count > 0)
            {
              if (selectedUnits[0].UnitType == UnitType.Character && selectedUnits[0].HumanControlledUnit)
                {
                    var unit = selectedUnits[0].GetComponent<Character>();
                    unit.SetTarget(hit);
                }
            }                                      
        }
    }

    private void CheckRightMouseClick()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame && !BuildMode.IsActive)
        {
            if (selectedUnits.Count > 0)
            {
                DeselectUnit();
            }
        }
    }

    #endregion

    #region Select&Deselect

    public void SelectUnit(Unit unit)
    {
        selectedUnits.Add(unit);
        SelectionCircleVisibility(true);
        ObjectSelected?.Invoke(selectedUnits[0].gameObject);
    }

    public void DeselectUnit()
    {
        SelectionCircleVisibility(false);
        selectedUnits.Clear();
        ObjectDeselected?.Invoke();
    }

    private void DeselectIfUnitIsDead(GameObject obj)
    {
        if (selectedUnits[0] == obj)
        {
            DeselectUnit();
        }
    }

    #endregion

    #region Visuals
    private void ShowHealthbar()
    {
        var selectable = hit.transform.GetComponent<Unit>();
        if (selectable)
        {
            hoveredUnit = selectable;
            hoveredUnit.ChangeHealthBarVisibility(true);
        }
        else
        {
            if (hoveredUnit != null)
            {
                hoveredUnit.ChangeHealthBarVisibility(false);
                hoveredUnit = null;
            }
        }        
    }

    private void SelectionCircleVisibility(bool visible)
    {
        if (selectedUnits.Count == 0) return;

        foreach (var unit in selectedUnits)
        {             
            unit.ChangeSelectionCircleVisibility(visible);         
        }
    }

    #endregion

    #region Pause

    public void Pause()
    {
        pause = true;
        StartCoroutine(ShortPause());
    }

    private IEnumerator ShortPause()
    {
        yield return new WaitForSeconds(0.2f);
        pause = false;
    }

    #endregion
}

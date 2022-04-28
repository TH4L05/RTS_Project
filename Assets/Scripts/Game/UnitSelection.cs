using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UnitSelection : MonoBehaviour
{
    #region Actions

    public static Action<GameObject> ObjectSelected;
    public static Action ObjectDeselected;
    //public static Action<Unit, bool> UnitOnSelection;
    public static Action<Unit, bool> UnitOnHover;

    #endregion

    #region SerializedFields

    [SerializeField] private LayerMask unitLayer;
    [SerializeField] private LayerMask groundLayer;

    #endregion

    #region PrivateFields

    private Camera cam;
    private List<Unit> selectedUnits = new List<Unit>();
    private Unit hoveredUnit;
    private bool paused;
    private bool isPressed;
    private float time;

    #endregion

    #region PublicFields

    [Header("TEST")]
    public Vector3 pos1 = Vector3.zero;
    public Vector3 pos2 = Vector3.zero;

    private RaycastHit rayhit1;
    private RaycastHit rayhit2;

    public GameObject obj1;
    public GameObject obj2;

    #endregion

    #region UnityFunctions

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnEnable()
    {        
        Unit.UnitIsDead += DeselectIfUnitIsDead;
    }

    private void OnDisable()
    {
        Unit.UnitIsDead -= DeselectIfUnitIsDead;
    }

    void Update()
    {
        if (paused) return;
        if (BuildMode.IsActive) return;

        ButtonPressed();       
        ShowHealthbar();      
        Deselection();
    }

    private void OnGUI()
    {
        if (isPressed == true)
        {
            Color color = new Color(0.25f, 0.80f, 0.50f, 0.50f);
            var rect = Utils.GetScreenRect(pos1, pos2);
            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();

            Utils.DrawScreenRect(rect, color, texture);
        }
    }

    #endregion

    #region LeftButtonState

    private void ButtonPressed()
    {
        ButtonWasPressedThisFrame();
        ButtonWasRealeasedThisFrame();
        ButtonIsPressed();
    }

    private void ButtonWasPressedThisFrame()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            obj1 = null;
            obj2 = null;
            time = 0f;

            Vector2 mousePosition = Utils.GetMousePosition();
            pos1 = mousePosition;

            Ray ray = cam.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 999f))
            {
                rayhit1 = hit;
                obj1 = rayhit1.collider.gameObject;

            }

            if (Physics.Raycast(ray, out hit, 999f, unitLayer))
            {
                rayhit2 = hit;
                obj2 = rayhit2.collider.gameObject;
            }
        }
    }

    private void ButtonWasRealeasedThisFrame()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            StopAllCoroutines();
            isPressed = false;

            pos2 = Utils.GetMousePosition();
            LeftMouseButtonEvent();
        }
    }

    private void ButtonIsPressed()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            if (!isPressed)
            {
                isPressed = true;
                StartCoroutine(MouseButtonIsPressed());
            }
        }
    }

    #endregion

    private IEnumerator MouseButtonIsPressed()
    {
        while (isPressed)
        {
            time += Time.deltaTime;
            pos2 = Utils.GetMousePosition();
            yield return null;
        }
    }

    private void LeftMouseButtonEvent()
    { 
        if (selectedUnits.Count > 0 && time < 0.25f)
        {
            SelectionTask();
        }
        else if (time < 0.25f)
        {
            SingleSelection();
        }
        else
        {
            DragSelection();
        }         
    }

    private void SelectionTask()
    {   
        foreach (var selectedUnit in selectedUnits)
        {
            Debug.Log("TEST");
            if (selectedUnit.UnitData.Type == UnitType.Character && selectedUnit.HumanControlledUnit)
            {
                Debug.Log("TESTTTT");
                var character = selectedUnit.GetComponent<Character>();
                character.SetTarget(obj1,rayhit1);
            }
        }

        if (obj2 != null)
        {
            var unit = obj2.GetComponent<Unit>();
            if(unit.HumanControlledUnit) SingleSelection();
        }
        //SingleSelection();
    }

    #region Selection

    private void SingleSelection()
    {
        if (obj2 == null) return;
        DeselectUnits();
        var unit = obj2.GetComponent<Unit>();
        if(unit.HumanControlledUnit) SelectUnit(unit);
    }

    private void DragSelection()
    {
        DeselectUnits();

        Vector3[] verticiesBottom = new Vector3[4];
        Vector3[] verticiesTop = new Vector3[4];
        Vector3[] vec = new Vector3[4];

        var bottomLeft = Vector3.Min(pos1, pos2);
        var topRight = Vector3.Max(pos1, pos2);

        // 0 = top left; 1 = top right; 2 = bottom left; 3 = bottom right;
        Vector2[] corners =
        {
            new Vector2(bottomLeft.x, topRight.y),
            new Vector2(topRight.x, topRight.y),
            new Vector2(bottomLeft.x, bottomLeft.y),
            new Vector2(topRight.x, bottomLeft.y)
        };

        int index = 0;
        foreach (Vector2 corner in corners)
        {
            Ray ray = Camera.main.ScreenPointToRay(corner);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 50000.0f, groundLayer))
            {
                verticiesBottom[index] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                vec[index] = ray.origin - hit.point;

                if (index == 0)
                {
                    Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), hit.point, Color.red, 2.0f);
                }
                else if (index == 1)
                {

                    Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), hit.point, Color.green, 2.0f);
                }
                else if (index == 2)
                {

                    Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), hit.point, Color.magenta, 2.0f);
                }
                else
                {
                    Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), hit.point, Color.cyan, 2.0f);
                }
            }
            index++;
        }

        index = 0;
        foreach (Vector3 vert in verticiesBottom)
        {
            verticiesTop[index] = new Vector3(vert.x, vert.y + 10f, vert.z);
            Debug.DrawLine(Camera.main.ScreenToWorldPoint(corners[index]), verticiesTop[index], Color.black, 2.0f);
            index++;
        }

        var distance1 = Vector3.Distance(verticiesBottom[0], verticiesBottom[1]) / 2;
        var distance2 = Vector3.Distance(verticiesBottom[2], verticiesBottom[3]) / 2;
        Vector3 v1 = Vector3.MoveTowards(verticiesBottom[0], verticiesBottom[1], distance1);
        Vector3 v2 = Vector3.MoveTowards(verticiesBottom[2], verticiesBottom[3], distance2);
        var distance3 = Vector3.Distance(v1, v2);
        Vector3 v3 = Vector3.MoveTowards(v1, v2, distance3);
        Vector3 centerPosition = new Vector3(v3.x, v3.y + 5f, v3.z);

        Collider[] colliders = Physics.OverlapBox(centerPosition, new Vector3(distance1, 5f, distance3), Quaternion.identity, unitLayer);
        MulitpleSelection(colliders);
    }

    private void Deselection()
    {      
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (selectedUnits.Count == 0) return;  
            obj1 = null;
            obj2 = null;
            DeselectUnits();         
        }
    }

    #endregion

    #region Select

    public void SelectUnit(Unit unit)
    {
        selectedUnits.Add(unit);
        unit.OnSelect();
        ObjectSelected?.Invoke(selectedUnits[0].gameObject);        
    }

    private void MulitpleSelection(Collider[] colliders)
    {
        if(colliders.Length == 0) return;
        if (colliders.Length == 1)
        {
            var unit = colliders[0].gameObject.GetComponent<Unit>();
            if(unit.HumanControlledUnit) SelectUnit(unit);
        }

        foreach (Collider collider in colliders)
        {

            var unit = collider.gameObject.GetComponent<Unit>();
            unit.OnSelect();
            selectedUnits.Add(unit);
        }
    }

    #endregion

    #region Deselect

    public void DeselectUnits()
    {
        foreach (var unit in selectedUnits)
        {
            unit.OnDeselect();
        }

        selectedUnits.Clear();
        ObjectDeselected?.Invoke();
    }

    private void DeselectIfUnitIsDead(GameObject obj)
    {
        if (selectedUnits.Count > 0)
        {
            DeselectUnits();
        }
    }

    #endregion

    #region Visuals
    private void ShowHealthbar()
    {
        Vector2 mousePosition = Utils.GetMousePosition();
        Ray ray = cam.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 999f, unitLayer))
        {
            hoveredUnit = hit.transform.GetComponent<Unit>();
            UnitOnHover?.Invoke(hoveredUnit, true);
        }
        else
        {
            if (hoveredUnit != null)
            {
                UnitOnHover?.Invoke(hoveredUnit, false);
                hoveredUnit = null;
            }
        } 
    }

    #endregion

    #region Pause

    public void Pause(bool pause)
    {
        paused = pause;
        isPressed = false;
    }

    public void ShortPause()
    {
        paused = true;
        isPressed = false;
        StopAllCoroutines();
        StartCoroutine(PauseShort());
    }

    private IEnumerator PauseShort()
    {
        yield return new WaitForSeconds(0.2f);
        paused = false;
    }

    #endregion
}

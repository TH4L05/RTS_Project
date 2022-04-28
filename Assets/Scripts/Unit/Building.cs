

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Building : Unit
{
    #region Actions

    public static Action<GameObject, List<FillTest>> UpdateFill;

    #endregion

    #region SerializedFields

    [SerializeField] private Transform unitSpawn;
    [SerializeField] private Transform gatheringPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int maxBuildSlots;
    public List<FillTest> activeFills = new List<FillTest>();

    #endregion

    #region PrivateFields

    private BuildingData data => unitData as BuildingData;
    private bool changeGatheringPosition;
    private Queue<GameObject> buildQueue;
    private bool onSelection;
    
    #endregion

    #region PublicFields

    public BuildingData Data => data;
    public Transform UnitSpawn => unitSpawn;
    public int buildCount => buildQueue.Count;

    #endregion

    #region Setup

    protected override void StartSetup()
    {
        base.StartSetup();       
        buildQueue = new Queue<GameObject>();

        if (unitSpawn != null)
        {
            unitSpawn.position = new Vector3(unitSpawn.position.x, transform.position.y, unitSpawn.position.z);
        }

        if (gatheringPoint != null)
        {
            gatheringPoint.gameObject.SetActive(false);
            gatheringPoint.position = new Vector3(unitSpawn.position.x, unitSpawn.position.y + 1.25f, unitSpawn.position.z);
        }
    }

    protected override void AdditionalSetup()
    {
        base.AdditionalSetup();
        if (data.BuildingType == BuildingType.ResourceProduction)
        {
            InvokeRepeating("ResourceProduced", 1, data.ProductionSpeed);
        }

        if (data.ProvideResourcesOnBuild == null) return;
        ProvideResources();
    }

    #endregion

    private void Update()
    {
        if (onSelection)
        {
            UpdateFill?.Invoke(gameObject, activeFills);
        }
    }

    private void ProvideResources()
    {
        foreach (var resource in data.ProvideResourcesOnBuild)
        {
            if (humanConrolledUnit)
            {
                ResourceManager.GainResource?.Invoke(owner, resource.ResoureData.Type, resource.amount, true);
            }
            else
            {
                ResourceManager.GainResource?.Invoke(owner, resource.ResoureData.Type, resource.amount, false);
            }
        }
    }

    private void ResourceProduced()
    {
        if (humanConrolledUnit)
        {
            ResourceManager.GainResource?.Invoke(owner, data.ProducedResource, data.ProductionAmount, true);
        }
        else
        {
            ResourceManager.GainResource?.Invoke(owner, data.ProducedResource, data.ProductionAmount, false);
        }       
    }

    public void ChangeGatheringPosition()
    {
        if (unitSpawn == null) return;
        changeGatheringPosition = true;
        StartCoroutine("Wait");
        InvokeRepeating("SetGatheringPosition", 0, 0.1f);
        Game.Instance.Unitselection.Pause(true);
    }

    public void SetGatheringPosition()
    {
        Game.Instance.Unitselection.Pause(true);

        var cam = Camera.main;
        Vector2 mousePosition = Utils.GetMousePosition();
        Ray ray = cam.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 999f, groundLayer))
        {
            gatheringPoint.position = new Vector3(hit.point.x, hit.point.y + 1.25f, hit.point.z);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            changeGatheringPosition = false;
            CancelInvoke("SetGatheringPosition");
            Game.Instance.Unitselection.Pause(false);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        InvokeRepeating("SetGatheringPosition", 0, 0.025f);
    }

    public void BuildNewUnit(GameObject obj)
    {
        var unit = obj.GetComponent<Unit>();
        var data = Utils.GetUnitData(unit);

        if (buildQueue.Count == maxBuildSlots -1)
        {
            Debug.LogError("NO SLOT AVIALABLE");
            return;
        }

        buildQueue.Enqueue(obj);  
        StartCoroutine(CreateNewUnit(obj));
        UnitSelection.ObjectSelected?.Invoke(gameObject);
    }

    IEnumerator CreateNewUnit(GameObject unitTemplate)
    {           
        var unit = unitTemplate.GetComponent<Unit>();
        var data = Utils.GetUnitData(unit);
        var name = data.Name + (buildCount);
        var job = new FillTest(name, data.ActionButtonIcon);
        activeFills.Add(job);

        float fillamount = 0f;
        float updateAmount = 1 / data.BuildTime / 60;

        while (fillamount < 1)
        {
            fillamount += updateAmount;

            foreach (var item in activeFills)
            {
                if (item.name == name)
                {
                    item.fillamount = fillamount;
                }
            }
            yield return null;
        }

        //yield return new WaitForSeconds(data.BuildTime);

        var newUnit = Instantiate(unitTemplate, unitSpawn.position, Quaternion.identity);

        if (gatheringPoint != null)
        {
            var character = newUnit.GetComponent<Character>();
            character.SetTarget(gatheringPoint.position);
        }
       
        Game.Instance.PlayerManager.AddUnit(newUnit.GetComponent<Unit>(), PlayerType.Human);
        activeFills.Remove(job);       
        buildQueue.Dequeue();      
    }

    protected override void ChangeSelectionVisibility( bool visible)
    {
        base.ChangeSelectionVisibility(visible);

        if (gatheringPoint == null) return;
        /*if(this.gameObject != unit.gameObject) return;
        if (unit.UnitData.Type != UnitType.Building) return;*/

        //Debug.Log("BuildingVisible" + visible);
        gatheringPoint.gameObject.SetActive(visible);
    }

    public override void OnSelect()
    {
        base.OnSelect();
        onSelection = true;
    }

    public override void OnDeselect()
    {
        base.OnDeselect();
        onSelection = false;
    }

}


[System.Serializable]
public class FillTest
{
    public string name;
    public float fillamount;
    public float fillamountTest;
    public Sprite sprite;

    public FillTest(string name, Sprite sprite)
    {
        this.name = name;
        this.sprite = sprite;
        Start();
    }

    void Start()
    {
        Game.Instance.StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        while (fillamountTest < 1)
        {
            fillamountTest += 0.0033f;
            yield return null;
        }
    }
}
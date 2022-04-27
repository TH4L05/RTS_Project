

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Building : Unit
{
    #region Actions
    #endregion

    #region SerializedFields

    [SerializeField] private Transform unitSpawn;
    [SerializeField] private Transform gatheringPoint;
    [SerializeField] private LayerMask groundLayer;

    #endregion

    #region PrivateFields

    private BuildingData data => unitData as BuildingData;
    private bool changeGatheringPosition;
    private Queue<GameObject> buildQueue;
    
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
        gatheringPoint.gameObject.SetActive(false);
        unitSpawn.position = new Vector3( unitSpawn.position.x, transform.position.y, unitSpawn.position.z);
        gatheringPoint.position = new Vector3(unitSpawn.position.x, unitSpawn.position.y + 1.25f , unitSpawn.position.z);
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
        Game.Instance.SelectionHandler.Pause(true);
    }

    public void SetGatheringPosition()
    {
        Game.Instance.SelectionHandler.Pause(true);

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
            Game.Instance.SelectionHandler.Pause(false);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        InvokeRepeating("SetGatheringPosition", 0, 0.025f);
    }

    public void AddToQueue(GameObject obj)
    {
        buildQueue.Enqueue(obj);
        StartCoroutine(CreateNewUnit(obj));
        UnitSelection.ObjectSelected?.Invoke(gameObject);
    }

    IEnumerator CreateNewUnit(GameObject unitTemplate)
    {
        var unit = unitTemplate.GetComponent<Unit>();
        var data = Utils.GetUnitData(unit);

        yield return new WaitForSeconds(data.BuildTime);

        var newUnit = Instantiate(unitTemplate, unitSpawn.position, Quaternion.identity);

        if (gatheringPoint != null)
        {
            var character = newUnit.GetComponent<Character>();
            character.SetTarget(gatheringPoint.position);
        }
       
        Game.Instance.PlayerManager.AddUnit(newUnit.GetComponent<Unit>(), PlayerType.Human);
        buildQueue.Dequeue();
    }

    protected override void ChangeSelectionVisibility(Unit unit, bool visible)
    {
        base.ChangeSelectionVisibility(unit, visible);

        if (gatheringPoint == null) return;
        if (unit.UnitData.Type != UnitType.Building) return;

        Debug.Log("BuildingVisible" + visible);
        gatheringPoint.gameObject.SetActive(visible);
    }
}

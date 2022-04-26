

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Building : Unit
{
    #region SerializedFields

    [SerializeField] private BuildingData data;
    [SerializeField] private Transform unitSpawn;
    [SerializeField] private Transform gatheringPoint;
    [SerializeField] private LayerMask groundLayer;
    private bool changeGatheringPosition;
    public Queue<GameObject> buildQueue;

    #endregion

    #region PublicFields

    public BuildingData Data => data;
    public Transform UnitSpawn => unitSpawn;

    #endregion

    #region Setup

    protected override void StartSetup()
    {
        base.StartSetup();      
        currentHealth = data.HealthMax; 
        buildQueue = new Queue<GameObject>();
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

    /*protected override void SetUnitType()
    {
        unitType = data.UType;
    }*/

    #endregion

    private void OnDrawGizmosSelected()
    {
        if(unitSpawn == null) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(unitSpawn.position, new Vector3(0.5f, 0.5f, 0.5f));
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (healthBar != null) healthBar.UpdateValue(currentHealth, data.HealthMax);
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
        //InvokeRepeating("SetGatheringPosition", 0, 0.1f);
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
        SelectionHandler.ObjectSelected?.Invoke(gameObject);
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
}

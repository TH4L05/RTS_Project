using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class BuildMode : MonoBehaviour
{
    public static bool IsActive;
    private GameObject activeObject;
    private GameObject tempObject;
    [SerializeField] private Material previewMaterialIn;
    [SerializeField] private Material previewMaterialOut;
    [SerializeField] private LayerMask groundLayer;
    private Camera cam;
    private RaycastHit hit;
    private bool onGrond;
    private bool canBuild;
    private PlayerString player;

    #region UnityFunctions

    void Start()
    {
        player = Game.Instance.PlayerManager.GetPlayerStringFromPlayer(PlayerType.Human);
        cam = Camera.main;
    }
   
    void Update()
    {
        if (!IsActive) return;
        LeftBuildMode();

        if (tempObject != null)
        {         
            Ray ray = cam.ScreenPointToRay(Game.Instance.GetMousePosition());
            
            if (Physics.Raycast(ray, out hit, 999f, groundLayer))
            {
                Vector3 pos = new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z);
                tempObject.transform.position = pos;
                CheckIfPossibleToBuild();
            }
                  
        }

        SpawnAtMousePosition();
    }

    #endregion

    public void ActivateMode(GameObject obj)
    {
        activeObject = obj;
        IsActive = true;
        CreateTempBuilding(obj);
        //tempObject = Instantiate(activeObject);
        //tempObject.name = "TempBuilding";
    }

    private void CreateTempBuilding(GameObject obj)
    {
        //tempObject = new GameObject("TempBuilding");
        //var tempMesh = tempObject.AddComponent<MeshFilter>();
        //var mr = tempObject.AddComponent<MeshRenderer>();
        /*var mesh = obj.AddComponent<MeshFilter>().sharedMesh;
        tempMesh = mesh;*/

        tempObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tempObject.name = "TempBuilding";
        var mr = tempObject.GetComponent<MeshRenderer>();
        mr.material = previewMaterialIn;
    }

    private void CheckIfPossibleToBuild()
    {
        Collider[] objOnHitPoint = Physics.OverlapSphere(hit.point, 2f);

        for (int i = 0; i < objOnHitPoint.Length; i++)
        {
            if (objOnHitPoint[i].gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                //Debug.Log(objOnHitPoint[i].gameObject.name);
                continue;
            }
            else if (objOnHitPoint[i].gameObject.name == "TempBuilding")
            {
                continue;
            }
            else
            {
                //Debug.LogError(objOnHitPoint[i].gameObject.name);
                onGrond = false;
                tempObject.GetComponent<MeshRenderer>().material = previewMaterialOut;
                return;
            }
        }
        tempObject.GetComponent<MeshRenderer>().material = previewMaterialIn;
        onGrond = true;
    }

    private void SpawnAtMousePosition()
    {       
        if (Mouse.current.leftButton.wasPressedThisFrame && onGrond)
        {
            if (activeObject == null) return;

            Vector3 spawnPosition = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);

            if (ResRequirementsMet())
            {
                var newBuilding = Instantiate(activeObject, spawnPosition, Quaternion.identity);           
                Game.Instance.PlayerManager.AddUnit(newBuilding.GetComponent<Building>(), PlayerType.Human);
                ConsumeRequiredRes();
                //Debug.Log(activeObject.GetComponent<Unit>().name);
            }
            else
            {
                Debug.LogError("NOT ENGOUGH RESOURCES TO BUILD");
            }
        }
    }

    private bool ResRequirementsMet()
    {
        var rqRes = activeObject.GetComponent<Building>().Data.RequiredResources;

        foreach (var resourceRequirement in rqRes)
        {
            if (Game.Instance.PlayerManager.CheckResourceRequirement(player, resourceRequirement.amount, resourceRequirement.ResoureData.Type))
            {
                continue;
            }
            else
            {
                return false;
            }

        }
        return true;
    }

    private void ConsumeRequiredRes()
    {
        var rqRes = activeObject.GetComponent<Building>().Data.RequiredResources;

        foreach (var resourceRequirement in rqRes)
        {
            ResourceManager.RemoveResource(player, resourceRequirement.ResoureData.Type, resourceRequirement.amount, true);
        }      
    }

    private void LeftBuildMode()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            IsActive = false;
            Destroy(tempObject);
            tempObject = null;
            activeObject = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(hit.point, 2f);
    }

}

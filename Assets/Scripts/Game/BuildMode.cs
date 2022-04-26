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
        RightMouseButtonPressed();

        if (tempObject != null)
        {         
            Ray ray = cam.ScreenPointToRay(Utils.GetMousePosition());
            
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
        tempObject.transform.localScale = new Vector3(5f, 5f, 5f);
        var mr = tempObject.GetComponent<MeshRenderer>();
        mr.material = previewMaterialIn;
    }

    private void CheckIfPossibleToBuild()
    {
        Collider[] objOnHitPoint = Physics.OverlapBox(hit.point, new Vector3(3.5f, 3.5f, 3.5f), Quaternion.identity);

        for (int i = 0; i < objOnHitPoint.Length; i++)
        {
            if (objOnHitPoint[i].gameObject.layer == LayerMask.NameToLayer("Ground") |
                objOnHitPoint[i].gameObject.name == "TempBuilding")
            {
                continue;
            }
            else
            {
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
                LeftBuildMode();
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

    private void RightMouseButtonPressed()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            LeftBuildMode();
        }
    }

    private void LeftBuildMode()
    {
        IsActive = false;
        Destroy(tempObject);
        tempObject = null;
        activeObject = null;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(hit.point, new Vector3(7f, 7f, 7f));
    }

}

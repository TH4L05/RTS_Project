using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class BuildMode : MonoBehaviour
{
    public static bool IsActive;
    private GameObject activeObject;
    private GameObject ghostObject;
    [SerializeField] private Material previewMaterialIn;
    [SerializeField] private Material previewMaterialOut;
    [SerializeField] private LayerMask groundLayer;
    private Camera cam;
    private RaycastHit hit;
    private bool onGrond;
    private bool canBuild;
    private PlayerString player;
    private List<Renderer> ghostRenderers = new List<Renderer>();

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

        if (ghostObject != null)
        {         
            Ray ray = cam.ScreenPointToRay(Utils.GetMousePosition());
            
            if (Physics.Raycast(ray, out hit, 999f, groundLayer))
            {
                Vector3 pos = hit.point;
                ghostObject.transform.position = pos;
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
        CreateGhostBuilding(obj);
    }

    private void CreateGhostBuilding(GameObject obj)
    {
        ghostRenderers.Clear();
        ghostObject = new GameObject("TempBuilding");
        var model = new GameObject("Model");
        model.transform.parent = ghostObject.transform;

        var modelCount = activeObject.transform.GetChild(0).transform.childCount;
        Debug.Log(modelCount);

        for (int i = 0; i < modelCount; i++)
        {
            GameObject ng = null;
            ng = new GameObject(i.ToString());
            ng.transform.parent = ghostObject.transform.GetChild(0).transform;

            var modelObj = activeObject.transform.GetChild(0).GetChild(i);

            ng.transform.localPosition = modelObj.transform.localPosition;
            ng.transform.rotation = modelObj.transform.rotation;
            ng.transform.localScale = modelObj.transform.localScale;

            var mf = ng.AddComponent<MeshFilter>();
            mf.sharedMesh = modelObj.GetComponent<MeshFilter>().sharedMesh;

            var mr = ng.AddComponent<MeshRenderer>();

            Material[] materials = modelObj.GetComponent<MeshRenderer>().sharedMaterials;          
            for (int m = 0; m < materials.Length; m++)
            {
                materials[m] = previewMaterialIn;
            }
            mr.sharedMaterials = materials;
        }


        //var tempMesh = tempObject.AddComponent<MeshFilter>();
        //var mr = tempObject.AddComponent<MeshRenderer>();
        /*var mesh = obj.AddComponent<MeshFilter>().sharedMesh;
        tempMesh = mesh;*/

        //ghostObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //ghostObject.name = "TempBuilding";
        //ghostObject.transform.localScale = new Vector3(5f, 5f, 5f);
        //var mr = ghostObject.GetComponent<MeshRenderer>();
        //mr.material = previewMaterialIn;
    }

    private void CheckIfPossibleToBuild()
    {
        Collider[] objOnHitPoint = Physics.OverlapBox(hit.point, new Vector3(3.5f, 3.5f, 3.5f), Quaternion.identity);
        var model = ghostObject.transform.GetChild(0);
        var meshRenderers = model.GetComponentsInChildren<Renderer>();

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

                foreach (var renderer in meshRenderers)
                {
                    Material[] materials = renderer.materials;
                    for (int m = 0; m < materials.Length; m++)
                    {
                        materials[m] = previewMaterialOut;
                    }

                    renderer.materials = materials;
                }

                //ghostObject.GetComponent<MeshRenderer>().material = previewMaterialOut;
                return;
            }
        }


        foreach (var renderer in meshRenderers)
        {
            Material[] materials = renderer.materials;
            for (int m = 0; m < materials.Length; m++)
            {
                materials[m] = previewMaterialIn;
            }

            renderer.materials = materials;
        }

        //ghostObject.GetComponent<MeshRenderer>().material = previewMaterialIn;
        onGrond = true;
    }

    private void SpawnAtMousePosition()
    {       
        if (Mouse.current.leftButton.wasPressedThisFrame && onGrond)
        {
            if (activeObject == null) return;

            Vector3 spawnPosition = hit.point;

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
        Destroy(ghostObject);
        ghostObject = null;
        activeObject = null;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(hit.point, new Vector3(7f, 7f, 7f));
    }

}

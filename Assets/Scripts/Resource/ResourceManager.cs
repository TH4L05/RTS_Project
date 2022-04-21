using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    //private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    public static Action<ResourceType, int> GainResource;
    public static Action<ResourceType, int> RemoveResource;
    public static Action<ResourceType> ResourceAmountCheck;

    [SerializeField] private List<ResourceSlot> resourceSlots = new List<ResourceSlot>();
    private static int am;


    private void Awake()
    {
        GainResource += UpdateResourceAmountPlus;
        RemoveResource += UpdateResourceAmountMinus;
    }

    private void Start()
    {
        foreach (var resource in resourceSlots)
        {
            resource.IncreaseValue(0);
            resource.SetSprite();
        }
    }

    private void OnDestroy()
    {
        GainResource -= UpdateResourceAmountPlus;
        RemoveResource -= UpdateResourceAmountMinus;
    }

    private void UpdateResourceAmountPlus(ResourceType type, int amount)
    {
        foreach (var resource in resourceSlots)
        {
            if (resource.ResourceType == type)
            {
                resource.IncreaseValue(amount);
                return;
            }
        }
    }

    private void UpdateResourceAmountMinus(ResourceType type, int amount)
    {
        foreach (var resource in resourceSlots)
        {
            if (resource.ResourceType == type)
            {
                resource.DecreaseValue(amount);
                return;
            }
        }
    }

    public int GetResoureAmount(ResourceType type)
    {
        foreach (var resource in resourceSlots)
        {
            if (resource.ResourceType == type)
            {
                am = resource.Amount;
                return resource.Amount;
            }
        }
        return 0;
    }

    public bool CheckResourceRequirement(int requiredAmount, ResourceType type)
    {
        var resourceAmount = GetResoureAmount(type);

        if ( resourceAmount >= requiredAmount)
        {
            return true;
        }
        return false;
    }

}

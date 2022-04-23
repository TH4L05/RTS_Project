using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceManager /*: MonoBehaviour*/
{
    [SerializeField] private List<ResourceSlot> resources = new List<ResourceSlot>();
    private PlayerString player = PlayerString.Undefined;

    public static Action<PlayerString, ResourceType, int, bool> GainResource;
    public static Action<PlayerString, ResourceType, int, bool> RemoveResource;
    public static Action<ResourceType, int> UpdateInfo;

    public ResourceManager(PlayerString player, List<ResourceSlot> resources)
    {
        foreach (var res in resources)
        {
            var slot = new ResourceSlot(res.Data, res.MaxAmount);
            slot.IncreaseValue(res.Amount);
            this.resources.Add(slot);
        }

        this.player = player;

        Setup();
    }

    public void Setup()
    {
        foreach (var resource in resources)
        {
            resource.IncreaseValue(0);
        }

        GainResource += UpdateResourceAmountPlus;
        RemoveResource += UpdateResourceAmountMinus;
    }

    public void Destroy()
    {
        GainResource -= UpdateResourceAmountPlus;
        RemoveResource -= UpdateResourceAmountMinus;
    }

    private void UpdateResourceAmountPlus(PlayerString player, ResourceType type, int amount, bool updateInfo)
    {
        if (this.player == player)
        {
            foreach (var resource in resources)
            {
                if (resource.ResourceType == type)
                {
                    resource.IncreaseValue(amount);

                    if (updateInfo)
                    {
                        UpdateResourceInfo(resource);
                    }

                    return;
                }
            }
        }     
    }

    private void UpdateResourceAmountMinus(PlayerString player, ResourceType type, int amount, bool updateInfo)
    {
        if (this.player == player)
        {
            foreach (var resource in resources)
            {
                if (resource.ResourceType == type)
                {
                    resource.DecreaseValue(amount);

                    if (updateInfo)
                    {
                        UpdateResourceInfo(resource);
                    }

                    return;
                }
            }
        }
    }

    private void UpdateResourceInfo(ResourceSlot resource)
    {
        UpdateInfo?.Invoke(resource.ResourceType, resource.Amount);
    }

    public int GetResoureAmount(ResourceType type)
    {
        foreach (var resource in resources)
        {
            if (resource.ResourceType == type)
            {
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

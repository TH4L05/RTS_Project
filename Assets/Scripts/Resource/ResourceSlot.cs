using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ResourceSlot
{
    [SerializeField] private ResourceData data;
    [SerializeField] private int amount;
    [SerializeField] private int amountMax;
    [SerializeField] private ResourceInfo resourceInfo;

    public int Amount => amount;
    public ResourceType ResourceType => data.Type;
    public ResourceData Data => data;

    public void SetAmount(int amount)
    {
        this.amount = amount;
    }

    public void IncreaseValue(int amount)
    {
        if(amount < amountMax) this.amount += amount;
        if(amount > amountMax) this.amount = amountMax;
    }



    public void DecreaseValue(int amount)
    {
        if(amount > 0) this.amount -= amount;
        if(amount < 0) this.amount = 0;
    }
}

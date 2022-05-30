/// <author> Thomas Krahl </author>

using UnityEngine;


[System.Serializable]
public class ResourceSlot
{
    [SerializeField] private ResourceData data;
    [SerializeField] private int amount;
    [SerializeField] private int amountMax;

    public int Amount => amount;
    public int MaxAmount => amountMax;
    public ResourceType ResourceType => data.Type;
    public ResourceData Data => data;


    public ResourceSlot(ResourceData data, int amountMax)
    {
        this.data = data;
        this.amountMax = amountMax;
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

/// <author> Thomas Krahl </author>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class ResourceInfoSlot
{
    public Image icon;
    public TextMeshProUGUI amountText;
    public ResourceType resourceType;
}

public class ResourceInfo : MonoBehaviour
{
    [SerializeField] private List<ResourceInfoSlot> slots = new List<ResourceInfoSlot>();

    private void Start()
    {
        ResourceManager.UpdateInfo += UpdateAmount;
    }

    private void OnDestroy()
    {
        ResourceManager.UpdateInfo -= UpdateAmount;
    }

    public void Setup(List<ResourceSlot> resources)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            slots[i].icon.sprite = resources[i].Data.Icon;
            slots[i].resourceType = resources[i].Data.Type;
            slots[i].amountText.text = resources[i].Amount.ToString();
        }
    }

    public void UpdateAmount(ResourceType type, int amount)
    {
        foreach (var slot in slots)
        {
            if (slot.resourceType == type)
            {
                slot.amountText.text = amount.ToString();
            }
        }
    }
}

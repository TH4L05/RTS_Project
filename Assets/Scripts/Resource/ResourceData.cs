/// <author> Thomas Krahl </author>

using UnityEngine;

public enum ResourceType
{
    Undefined = -1,
    Wood,
    Gold,
    Food,
    Unit,
}

[CreateAssetMenu(fileName = "NewResourece", menuName = "Data/Resource")]
public class ResourceData : ScriptableObject
{
    [SerializeField] private ResourceType type = ResourceType.Undefined;
    [SerializeField] private Sprite icon;

    public ResourceType Type => type;
    public Sprite Icon => icon;



}

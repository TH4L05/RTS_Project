using UnityEngine;

public class Ability : ScriptableObject
{
    [SerializeField] protected new string name;
    [SerializeField] protected string tooltip;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected GameObject obj;

    public string Name => name;
    public string Tooltip => tooltip;
    public Sprite Icon => icon;

    public virtual void SetObject(GameObject obj)
    {
        this.obj = obj;
    }

    public virtual void DoAction()
    {
    }
}
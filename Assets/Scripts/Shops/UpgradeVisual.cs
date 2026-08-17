using UnityEngine;

public class UpgradeVisual : DraggableObject
{
    public void Start()
    {
        Awake();
    }


    protected override bool TryGetValidDropPosition(out Vector2 validPosition)
    {
        
        validPosition = transform.position;
        return false;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public abstract class DraggableObject : MonoBehaviour
{
    protected bool IsBeingPlaced = false;

    protected static bool PlacingObject = false;

    private Vector3 originalPosition;

    protected virtual void Awake()
    {
        originalPosition = transform.position;
    }

    private void OnMouseDown()
    {
        if (PlacingObject && !IsBeingPlaced)
            return;

        IsBeingPlaced = true;
        PlacingObject = true;

        OnStartDragging();
    }

    private void OnMouseUp()
    {
        if (!IsBeingPlaced)
            return;

        if (TryGetValidDropPosition(out Vector2 validPosition))
        {
            OnPlace(validPosition);

            IsBeingPlaced = false;
            PlacingObject = false;
        }
        else
        {
            IsBeingPlaced = false;
            PlacingObject = false;

            OnCancelDragging();
        }
    }

    protected virtual void Update()
    {
        if (IsBeingPlaced)
        {
            DragToMouse();
            OnDragging();
        }
        else
        {
            transform.position = originalPosition;
        }
    }

    private void DragToMouse()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(
                mouseScreenPosition.x,
                mouseScreenPosition.y,
                -Camera.main.transform.position.z
            )
        );

        mouseWorldPosition.z =  originalPosition.z - 2;
        transform.position = mouseWorldPosition;
    }

    protected virtual void OnStartDragging()
    {
    }

    protected virtual void OnDragging()
    {
    }

    protected virtual void OnPlace(Vector2 validPosition)
    {
    }


    protected virtual void OnCancelDragging()
    {
    }


    protected abstract bool TryGetValidDropPosition(out Vector2 validPosition);

    protected virtual void OnDisable()
    {
        if (IsBeingPlaced)
        {
            IsBeingPlaced = false;
            PlacingObject = false;
        }
    }
}
using UnityEngine;

public class ConsumableVisual : DraggableObject
{
    
    public TMPro.TextMeshPro name;
    public TMPro.TextMeshPro desc;
    public TMPro.TextMeshPro price;
    
    public enum PlacementType
    {
        Anywhere,
        OnBoard,
        
        OnUpgrade,
        OnConsumable
    }

    public PlacementType type;


    public bool IsShop = false;

    
    public void Start()
    {
        Awake();
        name.text = Consumable.name;
        desc.text = Consumable.description;
        if (IsShop)
        {
            price.gameObject.SetActive(true);
            price.text = Consumable.price().ToString();
        }
    }

    public Consumable Consumable;


    protected override bool TryGetValidDropPosition(out Vector2 validPosition)
    {
        validPosition = transform.position;
        
        Collider2D consumableAreaCollider = GameManager.Instance.consumableArea.GetComponent<Collider2D>();


        if ( Consumable == null || GameManager.Instance == null ||
            ScoreManagement.Instance == null || GameManager.Instance.consumableArea == null)
        {
            return false;
        }
        else if (!IsShop)
        {
            switch (type)
            {
                default: case PlacementType.Anywhere:

                    if (consumableAreaCollider != null &&
                        !consumableAreaCollider.OverlapPoint(transform.position)) return true;
                    
                break;
                
                
            }
            
        }


        return consumableAreaCollider != null &&
               consumableAreaCollider.OverlapPoint(transform.position) &&
               ScoreManagement.Instance.UnspentScore >= Consumable.price() &&
               GameManager.Instance.Consumables.Count < GameManager.Instance.MaxConsumables;
    }

    protected override void OnPlace(Vector2 validPosition)
    {
        if (IsShop && GameManager.Instance.AddConsumable(Consumable))
        {
            ScoreManagement.Instance.UnspentScore -= Consumable.price();
            Destroy(gameObject);
        }
        else if (!IsShop) Consumable.OnDrop(validPosition);
    }
}

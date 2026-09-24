using UnityEngine;

public class ConsumableVisual : DraggableObject
{
    
    public TMPro.TextMeshPro name;
    public TMPro.TextMeshPro desc;
    public TMPro.TextMeshPro price;

    public GameObject pulloutMenu;
    
    public enum PlacementType
    {
        Anywhere,
        OnBoard,
        OnBlockArea,
        
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
        
        if (!IsShop)
        {
            if (!Consumable.CanUse()) return false;
            
            switch (type)
            {
                default: case PlacementType.Anywhere:
                    if (consumableAreaCollider != null &&
                        !consumableAreaCollider.OverlapPoint(transform.position)) return true;
                break;
                case PlacementType.OnBoard:
                    Collider2D boardCollider = GameBoard.instance.GetComponent<Collider2D>();
                    if (boardCollider != null &&
                        boardCollider.OverlapPoint(transform.position)) return true;
                    break;
                case PlacementType.OnBlockArea:
                    Collider2D blockareacollider = BlockPlacementArea.instance.GetComponent<Collider2D>();
                    if (blockareacollider != null &&
                        blockareacollider.OverlapPoint(transform.position)) return true;
                    break;
                
            }

            return false;
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

    protected override void OnStartDragging()
    {
        //ToDo: For consumables dropped on upgrade, should swap to upgrade area instead when using but only if they arent marked as shop
        if (!GameManager.Instance.consumableArea.gameObject.activeInHierarchy) GameManager.Instance.areaswitcher.Switch();
        pulloutMenu.SetActive(true);
    }

    protected override void OnCancelDragging()
    {
        pulloutMenu.SetActive(false);
    }
}

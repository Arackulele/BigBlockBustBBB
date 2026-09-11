using UnityEngine;

public class ConsumableVisual : DraggableObject
{
    
    public TMPro.TextMeshPro name;
    public TMPro.TextMeshPro desc;
    public TMPro.TextMeshPro price;


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

        if (!IsShop || Consumable == null || GameManager.Instance == null ||
            ScoreManagement.Instance == null || GameManager.Instance.consumableArea == null)
        {
            return false;
        }

        Collider2D consumableAreaCollider = GameManager.Instance.consumableArea.GetComponent<Collider2D>();

        return consumableAreaCollider != null &&
               consumableAreaCollider.OverlapPoint(transform.position) &&
               ScoreManagement.Instance.UnspentScore >= Consumable.price() &&
               GameManager.Instance.Consumables.Count < GameManager.Instance.MaxConsumables;
    }

    protected override void OnPlace(Vector2 validPosition)
    {
        if (GameManager.Instance.AddConsumable(Consumable))
        {
            ScoreManagement.Instance.UnspentScore -= Consumable.price();
            Destroy(gameObject);
        }
    }
}

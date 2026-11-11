using UnityEngine;

public class UpgradeVisual : DraggableObject
{
    
    public TMPro.TextMeshPro name;
    public TMPro.TextMeshPro desc;
    public TMPro.TextMeshPro price;


    public bool IsShop = false;

    
    public void Start()
    {
        Awake();
        name.text = upgrade.name;
        desc.text = upgrade.description;
        if (IsShop)
        {
            price.gameObject.SetActive(true);
            price.text = upgrade.price().ToString();
        }
    }

    public Upgrade upgrade;


    protected override bool TryGetValidDropPosition(out Vector2 validPosition)
    {
        validPosition = transform.position;

        if (!IsShop )
        {
            if ( upgrade == null || GameManager.Instance == null || ScoreManagement.Instance == null || GameManager.Instance.upgradeArea == null) return false;
            if (RemoveAreaScript.colliderinstance != null && RemoveAreaScript.colliderinstance.OverlapPoint(transform.position)) return true;
        }

        Collider2D upgradeAreaCollider = GameManager.Instance.upgradeArea.GetComponent<Collider2D>();

        return upgradeAreaCollider != null &&
               upgradeAreaCollider.OverlapPoint(transform.position) &&
               ScoreManagement.Instance.UnspentScore >= upgrade.price() &&
               GameManager.Instance.Perks.Count < GameManager.Instance.MaxPerks &&
               !GameManager.Instance.Perks.Contains(upgrade);
    }

    protected override void OnPlace(Vector2 validPosition)
    {
        if (IsShop && GameManager.Instance.AddUpgrade(upgrade))
        {
            ScoreManagement.Instance.UnspentScore -= upgrade.price();
            Destroy(gameObject);
        }
        else
        {
            ScoreManagement.Instance.UnspentScore += upgrade.price() * 0.5f;
            Destroy(gameObject); 
        }
    }
    
    protected override void OnStartDragging()
    {
        if (!GameManager.Instance.upgradeArea.gameObject.activeInHierarchy) GameManager.Instance.areaswitcher.Switch();
    }
}

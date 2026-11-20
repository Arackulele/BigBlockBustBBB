using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UpgradeVisual : DraggableObject
{
    
    public TMPro.TextMeshPro name;
    public TMPro.TextMeshPro desc;
    public TMPro.TextMeshPro price;


    public bool IsShop = false;

    [SerializeField] private SpriteRenderer scroller;

    [SerializeField] private GameObject activateVisual;
    
    public void Start()
    {
        Awake();
        name.text = upgrade.name;
        desc.text = upgrade.description();

        if (IsShop)
        {
            price.color = Color.softYellow;
            price.text = upgrade.price().ToString();
        }
        else 
        {
            price.color = Color.darkOrange;
            price.text = (upgrade.price() * 0.5f).ToString();
        }
        
    }

    //this is lowk kinda hacky
    void FixedUpdate()
    {
        name.text = upgrade.name;
        desc.text = upgrade.description();
    }

    public Upgrade upgrade;

    public IEnumerator OnActivate()
    {
        Debug.Log("Upgrade was activated");
        Color orig = scroller.color;
        scroller.color = Color.white;
        activateVisual.SetActive(true);
        yield return new WaitForSeconds(0.38f);
        activateVisual.SetActive(false);
        scroller.color = orig;
    }


    protected override bool TryGetValidDropPosition(out Vector2 validPosition)
    {
        validPosition = transform.position;

        if (!IsShop)
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
            upgrade.Remove(); 
        }
    }
    
    
    protected override void OnStartDragging()
    {
        if (!GameManager.Instance.upgradeArea.gameObject.activeInHierarchy) GameManager.Instance.areaswitcher.Switch();
    }
}

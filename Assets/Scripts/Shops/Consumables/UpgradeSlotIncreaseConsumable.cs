using UnityEngine;

public class UpgradeSlotIncreaseConsumable : Consumable
{
    public override Rarity rarity { get => Rarity.Epic; }

    public override ConsumableVisual.PlacementType Placement { get => ConsumableVisual.PlacementType.Anywhere; }


    
    public override string name => "Golden Egg";

    public override string description => "Use to gain an additional Upgrade Slot, but gain 20% less score this run";

    public override void OnDrop(Vector2 pos)
    {
        GameManager.Instance.GlobalMultiplier -= 0.2;
        GameManager.Instance.ChangeMaxPerks(1);
        UsedUp();
    }
}

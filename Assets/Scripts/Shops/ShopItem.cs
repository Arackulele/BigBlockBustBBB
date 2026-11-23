using UnityEngine;

public class ShopItem : GameModifier
{
    [HideInInspector]
    public Sprite sprite;

    private double? cachedPrice;

    public virtual Rarity rarity { get; }
    
    public virtual float baseprice { get; }


    public override double price()
    {
        if (cachedPrice.HasValue)
            return cachedPrice.Value;

        double generatedPrice = Cost();
        

        
        cachedPrice = generatedPrice;
        return cachedPrice.Value;
    }

    public virtual float Cost()
    {
        return baseprice;
    }

    public void ResetPrice()
    {
        cachedPrice = null;
    }
}

    
    
public enum Rarity
{
    Common,
    Rare,
    Epic
}

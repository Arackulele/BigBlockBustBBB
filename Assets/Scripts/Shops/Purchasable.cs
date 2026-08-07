using UnityEngine;

public class Purchasable
{
    double Price = 5;

    GameObject visual;

    public virtual GameObject createVisual(Transform par) { return null; }
}

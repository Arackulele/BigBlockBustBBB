using UnityEngine;

public class Purchasable
{
    public virtual double price() { return 10; }

    GameObject visual;

    public virtual GameObject createVisual(Transform par) { return null; }
}

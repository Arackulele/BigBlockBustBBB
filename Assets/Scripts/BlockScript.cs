using UnityEngine;

public class BlockScript : MonoBehaviour
{

    private bool IsFilled = false;

    [SerializeField]
    private GameObject Block;

    public Color Color;

    [SerializeField]
    private GameObject hlight;

    [SerializeField]
    private ParticleSystem ClearParticle;

    public Vector2Int GridPos;

    public bool Clear()
    {
        Debug.Log("Attempting to clear Block");
        if (!IsFilled) return false;
        Block.SetActive(false);
        IsFilled = false;
        ClearParticle.startColor = Color;
        ClearParticle.Play();
        return true;
    }

    public bool Fill()
    {
        if (IsFilled) return false;
        Block.GetComponent<SpriteRenderer>().color = Color;
        Block.SetActive(true);
        IsFilled = true;
        return true;
    }

    public void SetColor(Color c)
    { Color = c; }

    public void highlight()
    {
        Block.transform.position = new Vector3(Block.transform.position.x, Block.transform.position.y, Block.transform.position.z - 0.5f);
        hlight.SetActive(true);
    }
    public void dehighlight()
    {
        Block.transform.position = new Vector3(Block.transform.position.x, Block.transform.position.y, Block.transform.position.z + 0.5f);
        hlight.SetActive(false);
    }


}

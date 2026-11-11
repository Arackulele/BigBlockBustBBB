using UnityEngine;

public class RemoveAreaScript : MonoBehaviour
{ 
    public static Collider2D colliderinstance;
    void Start()
    {
        colliderinstance = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

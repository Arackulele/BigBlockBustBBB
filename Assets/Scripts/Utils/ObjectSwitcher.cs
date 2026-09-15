using UnityEngine;

public class ObjectSwitcher : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        object1.SetActive(true);
        object2.SetActive(false);
        
    }

    // Update is called once per frame
    public void Switch()
    {
        if (object1.activeSelf) { object1.SetActive(false); object2.SetActive(true); }
        else  { object1.SetActive(true); object2.SetActive(false); }
        
    }
}

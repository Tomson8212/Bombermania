using UnityEngine;

public class Crate : MonoBehaviour
{
    
    public GameObject hiddenItemPrefab;

    public void DestroyCrate()
    {
        
        if (hiddenItemPrefab != null)
        {
            
            Instantiate(hiddenItemPrefab, transform.position, Quaternion.identity);
        }

        
        Destroy(gameObject);
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Goodbye", 1.0f);
    }

    // Update is called once per frame
    void Goodbye()
    {
        Destroy(this.gameObject); 
    }
}

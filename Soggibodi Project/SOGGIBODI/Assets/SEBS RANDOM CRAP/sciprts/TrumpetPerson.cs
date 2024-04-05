using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetPerson : MonoBehaviour
{
    [SerializeField] float spinSpeed;

    // Update is called once per frame
    void Update()
    {
        float rotationDelta = spinSpeed * Time.deltaTime;
        transform.eulerAngles =  new Vector3(transform.eulerAngles.x,
                                                transform.eulerAngles.y + rotationDelta,
                                                transform.eulerAngles.z);
    }

    public void Die()
    {
        // Instantiate some really horrible blood splatter and play a gross sound
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider _c)
    {
        
    }
}

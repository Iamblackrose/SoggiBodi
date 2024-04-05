using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HPPickup : MonoBehaviour
{
    [SerializeField] float spinSpeed;
    [SerializeField] float healAmount;

    // Update is called once per frame
    void Update()
    {
        float rotationDelta = spinSpeed * Time.deltaTime;
        transform.eulerAngles =  new Vector3(transform.eulerAngles.x,
                                                transform.eulerAngles.y + rotationDelta,
                                                transform.eulerAngles.z);
        
        // maybe do some funny shit here idk
        spinSpeed += Time.deltaTime;
    }

    void OnTriggerEnter(Collider _c){
        PlayerControls hitPlayer = _c.GetComponentInParent<PlayerControls>();
        if(hitPlayer){
            hitPlayer.RestoreHealth(healAmount);
            Destroy(this.gameObject);
        }
    }
}

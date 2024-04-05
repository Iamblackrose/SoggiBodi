using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainfulParticles : MonoBehaviour
{
    [SerializeField] float damageAmount = 1.0f;
    void OnParticleCollision(GameObject _c)
    {
        Debug.Log("Particle Collision Detected against " + _c.name);
        PlayerControls hitPlayer = _c.GetComponentInParent<PlayerControls>();
        
        if(hitPlayer){
            hitPlayer.TakeDamage(this.damageAmount);
        }
    }
}

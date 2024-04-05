using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MeleeWeapon
{
    [SerializeField] float knockbackStrength;
    private bool willKnockback = false;

    public override void OnActivate()
    {
        base.OnActivate();
        willKnockback = true;
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();
        willKnockback = false;
    }

    private void OnTriggerEnter(Collider _c)
        {
            Debug.Log(this.gameObject.name + " collided with " + _c.gameObject.name);
            PlayerControls hitPlayer = _c.GetComponentInParent<PlayerControls>();

            if(hitPlayer){
                Debug.Log("Hit " + hitPlayer.gameObject.name);
                if(hitPlayer.gameObject != this.owner){
                    hitPlayer.TakeDamage(this.damageAmount);

                    Vector3 knockbackDir = (hitPlayer.transform.position - this.owner.transform.position).normalized;
                    hitPlayer.GetHipsRB().AddForce(knockbackDir * knockbackStrength);
                    Instantiate(bloodSplatterVFXPrefab, _c.transform.position, Quaternion.LookRotation(knockbackDir));
                    if(willKnockback) {hitPlayer.Flop();}
                }
            }
            else{
                Debug.LogWarning("Could not find Player Controls");
            }

            if(_c.gameObject.TryGetComponent<TrumpetPerson>(out TrumpetPerson t_)){
            t_.Die();
        }
        }
}

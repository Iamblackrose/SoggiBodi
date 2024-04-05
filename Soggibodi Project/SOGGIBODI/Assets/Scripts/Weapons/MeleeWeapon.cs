using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] Collider weaponCollider;

    void Start()
    {
        weaponCollider.enabled = false;
    }

    public override void OnActivate()
    {
        weaponCollider.enabled = true;
    }

    public override void OnDeactivate()
    {
        weaponCollider.enabled = false;
    }

    /// <summary>
    /// NEEDS TO BE UPDATED WITH PLAYER SCRIPT TYPE REFERENCE
    /// </summary>
    /// <param name="otherCollider"></param>
    private void OnTriggerEnter(Collider _c)
    {
        Debug.Log(this.gameObject.name + " collided with " + _c.gameObject.name);
        PlayerControls hitPlayer = _c.GetComponentInParent<PlayerControls>();

        if(hitPlayer){
            Debug.Log("Hit " + hitPlayer.gameObject.name);
            if(hitPlayer.gameObject != this.owner){
                Vector3 splatterDir = (hitPlayer.transform.position - this.owner.transform.position).normalized;

                Instantiate(bloodSplatterVFXPrefab, _c.transform.position, Quaternion.LookRotation(splatterDir));
                hitPlayer.TakeDamage(this.damageAmount);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [Header("Firing Settings")]
    [SerializeField] bool isFiring;
    [SerializeField] private float fireInterval;

    private float lastFireTime;

    [Header("Projectile Settings")]
    [SerializeField] protected Transform firePosition;
    [SerializeField] protected GameObject projectilePrefab;

    #region Unity Funcs
    private void Start()
    {

    }

    void FixedUpdate()
    {
        if(isFiring && Time.time > lastFireTime){
            Instantiate(projectilePrefab, firePosition.position, Quaternion.identity);
            // Maybe add RB force here, if Projectile doesn't do it itself on spawn.

            lastFireTime = Time.time + fireInterval;
        }
    }

    private void OnDrawGizmos()
    {
        if (firePosition)
        {
            Gizmos.DrawSphere(firePosition.position, 1.0f);
        }
    }
    #endregion

    #region Weapon

    public override void OnActivate()
    {
        isFiring = true;
    }

    public override void OnDeactivate()
    {
        isFiring = false;
    }

    #endregion
}

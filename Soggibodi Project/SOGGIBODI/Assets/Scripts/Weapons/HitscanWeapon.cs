using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HitscanWeapon : Weapon
{
    [Header("Firing Settings")]
    [SerializeField] private bool isFiring;
    [SerializeField] private float fireInterval;

    private float lastFireTime;

    [Header("AOE Sphere Settings")]
    [SerializeField] protected Transform firePosition;
    [SerializeField] protected float castDistance;
    [SerializeField] protected float castWidth;

    [SerializeField] protected LayerMask playerLayer;

    [Header("VFX")]
    [SerializeField] private ParticleSystem particleVFX;
    [SerializeField] List<ParticleCollisionEvent> particleCollisions = new List<ParticleCollisionEvent>();

    #region Unity Funcs
    private void OnEnable()
    {
        particleVFX?.Stop();
    }

    private void Update()
    {
        if (!isFiring) return;

        if (Time.time > lastFireTime)
        {
            particleVFX?.Play();

            // RaycastHit[] hits = Physics.SphereCastAll(firePosition.position, castWidth, firePosition.forward, castDistance, playerLayer, QueryTriggerInteraction.Ignore);
            // if (hits.Length > 0)
            // {
            //     foreach (RaycastHit rHit in hits)
            //     {
            //         PlayerControls hitPlayer = rHit.collider.GetComponentInParent<PlayerControls>();
            //         if (hitPlayer.gameObject != this.owner)
            //         {
            //             hitPlayer.TakeDamage(this.damageAmount);
            //         }
            //     }
            // }

            lastFireTime = Time.time + fireInterval;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(firePosition.position, firePosition.forward * castDistance);

        Gizmos.DrawSphere(firePosition.position + firePosition.forward * castDistance, castWidth);
    }
    #endregion

    #region Weapon Funcs
    public override void OnActivate()
    {
        isFiring = true;

        particleVFX?.Play();
    }

    public override void OnDeactivate()
    {
        isFiring = false;

        particleVFX?.Stop();
    }
    #endregion
}

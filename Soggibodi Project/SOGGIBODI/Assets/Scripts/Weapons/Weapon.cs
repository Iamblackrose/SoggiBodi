using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public abstract class Weapon : MonoBehaviour
{
    [Header("Basic Fields")]
    public string weaponName;
    public string weaponDescription;

    [Header("Combat Fields")]
    [SerializeField] protected GameObject owner;
    [Tooltip("The amount of damage that the weapon does when activated")][SerializeField] protected float damageAmount;
    [SerializeField] protected GameObject bloodSplatterVFXPrefab;

    private void OnEnable()
    {
        //might need changing with weapon?
        SetOwner(GetComponentInParent<PlayerControls>().gameObject);
    }

    // Called when a player with the weapon extends their arm
    // no default behaviours, but will inherit to spew fire from a magic wand, put up a barrier hitbox, etc etc
    public virtual void OnActivate() { }

    // Called when a player with the weapon stops extending their arm
    // Undoes all the OnActivate() stuff
    public virtual void OnDeactivate() { }

    /// <summary>
    /// Enables any Weapon specific funcitonality, that requires an owner to be present.
    /// </summary>
    protected virtual void EnableOwnerRequired() { }

    /// <summary>
    /// Disables any Weapon specific functionality, that requires an owner to be present.
    /// </summary>
    protected virtual void DisableOwnerRequired() { }

    #region Owner Functions

    public void SetOwner(GameObject _newOwner)
    {
        this.owner = _newOwner;
    }

    #endregion
}

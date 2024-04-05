using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallbox : MonoBehaviour
{
    [Header("Fling Settings")]
    [SerializeField] private Transform flingTarget;

    private float flingForce = 100.0f;

    private void OnTriggerEnter(Collider col)
    {
        PlayerControls playerControls = col.GetComponentInParent<PlayerControls>();
        if (playerControls == null) return;

        Debug.Log("Player fell in fallbox");
        Rigidbody playerRB = playerControls.GetHipsRB();

        Launch(flingTarget, flingForce, playerRB);

        playerControls.Flop();
    }

    void Launch(Transform targetObject, float forceMagnitude, Rigidbody rb)
    {
        // Calculate direction towards the target
        Vector3 direction = targetObject.position - transform.position;

        // Normalize the direction vector to get a unit vector
        direction.Normalize();

        // Calculate the force vector
        Vector3 force = direction * forceMagnitude;

        // Apply the force to the Rigidbody
        rb.AddForce(force, ForceMode.Impulse);
    }
}

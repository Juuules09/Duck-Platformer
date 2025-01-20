using System;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float Force = 10f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Impulse(other);
        }
    }

    /// <summary>
    /// Applies a force to the player on collision.
    /// </summary>
    /// <param name="other">The player's collider.</param>
    void Impulse(Collider other)
    {
        // Reset the player's velocity
        other.GetComponent<TempGravity>().Velocity = Vector3.zero;
        // Apply the force upwards
        other.GetComponent<TempGravity>().Velocity += transform.up * Force;
    }
}

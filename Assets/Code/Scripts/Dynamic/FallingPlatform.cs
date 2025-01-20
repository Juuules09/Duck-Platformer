using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Called when the object begins touching another object.
    /// </summary>
    /// <param name="collision">The collision data.</param>
    void OnTriggerEnter(Collider collision)
    {
        // When the player lands on the platform, make the platform fall
        if (collision.gameObject.tag == "Player")
        {
            _rb.isKinematic = false;
        }
    }
}
using UnityEngine;

public class TempGravity : MonoBehaviour
{
    CharacterController _controller;
    public float Gravity = 9.81f;
    [SerializeField] float _airDrag = 0.25f;
    [SerializeField] float _groundDrag = 5f;

    public bool IsBumped;

    public Vector3 Velocity;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Applies gravity to the duck's movement.
    /// </summary>
    void FixedUpdate()
    {
        // Ground drag
        if (_controller.isGrounded)
        {
            // Linearly interpolate the x and z velocities to zero
            Velocity.x = Mathf.Lerp(Velocity.x, 0, _groundDrag * Time.deltaTime);
            Velocity.z = Mathf.Lerp(Velocity.z, 0, _groundDrag * Time.deltaTime);
        }
        else
        {
            // Air drag
            // Linearly interpolate the x and z velocities to zero
            Velocity.x = Mathf.Lerp(Velocity.x, 0, _airDrag * Time.deltaTime);
            Velocity.z = Mathf.Lerp(Velocity.z, 0, _airDrag * Time.deltaTime);
        }

        // Apply gravity
        Velocity.y -= Gravity * Time.deltaTime;

        // Move the character controller
        _controller.Move(Velocity * Time.deltaTime);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class DuckMovement : MonoBehaviour
{
    PlayerInput _playerInput;
    Rigidbody _rb;

    [SerializeField] float _movementSpeed;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Move"].performed += Move;
        _rb = GetComponent<Rigidbody>();
    }

    void Move(InputAction.CallbackContext context)
    {
        if (_playerInput != null)
        {
            Vector2 movementValue = context.ReadValue<Vector2>();
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            Vector3 movement = cameraForward * movementValue.y + Camera.main.transform.right * movementValue.x;
            _rb.velocity = movement * _movementSpeed;
            Rotate(context);
        }
    }

    void Rotate(InputAction.CallbackContext context)
    {
        if (_playerInput != null)
        {
            Vector2 movementValue = context.ReadValue<Vector2>();
            if (movementValue != Vector2.zero)
            {
                Vector3 direction = Camera.main.transform.forward * movementValue.y + Camera.main.transform.right * movementValue.x;
                direction.Normalize();
                direction.y = 0;
                Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.1f);
            }
        }
    }
}

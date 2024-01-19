using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float jumpSpeed;
    public float crouchJumpForce;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public float crouchSpeedMultiplier = 0.5f;
    public float crouchHeight = 0.5f;
    public float glideDuration = 5f; // Durée du planage en secondes
    private float originalHeight;
    private float originalCenterY;
    private float crouchingCenterY;
    private float ySpeed;
    private float glideTimer = 0f; // Chronomètre pour le planage
    private CharacterController controller;
    private bool isCrouching = false;
    private bool isGliding = false;
    private bool isInPlayMode = false;
    private bool ctrlZPressed = false;

    public Transform cameraTransform;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
        originalCenterY = controller.center.y;
        crouchingCenterY = originalCenterY - (originalHeight - crouchHeight) / 2f;
        ySpeed = 0f;

        // Assurez-vous que la caméra est correctement assignée manuellement dans l'éditeur Unity
        if (cameraTransform == null)
        {
            Debug.LogError("Camera not assigned to cameraTransform in PlayerMovement!");
        }

        // Vous pouvez initialement supposer que le jeu est en cours d'exécution
        // à moins que vous ne soyez en mode édition
        isInPlayMode = !Application.isEditor;
    }

    void Update()
    {
        HandleMovement();

        if (Input.GetKey(KeyCode.LeftControl))
        {
            StartCrouch();
        }
        else
        {
            StopCrouch();
        }

        CheckCeiling();

        // Ajout de la condition pour les actions spécifiques à CTRL + Z en mode de jeu
        if (isInPlayMode)
        {
            ctrlZPressed = Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z);

            if (ctrlZPressed)
            {
                // Actions spécifiques à CTRL + Z pendant le jeu
                // ...
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartGlide();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopGlide();
        }
    }

    void HandleMovement()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraForward * verticalMove + cameraRight * horizontalMove).normalized;

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotate = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotationSpeed * Time.deltaTime);
        }

        if (controller.isGrounded)
        {
            ySpeed = -0.5f;

            if (Input.GetButtonDown("Jump"))
            {
                if (isCrouching)
                {
                    ySpeed = Mathf.Sqrt(2f * crouchJumpForce * Mathf.Abs(Physics.gravity.y));
                }
                else
                {
                    ySpeed = jumpSpeed;
                }
            }
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;

            if (ySpeed < 0)
            {
                ySpeed += Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (ySpeed > 0 && !Input.GetButton("Jump"))
            {
                ySpeed += Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }

        float currentSpeed = isCrouching ? speed * crouchSpeedMultiplier : speed;

        if (isGliding)
        {
            ySpeed = -0.1f; // Ajustez la vitesse de planage ici

            glideTimer += Time.deltaTime;

            if (glideTimer >= glideDuration)
            {
                StopGlide();
            }
        }

        Vector3 vel = moveDirection * currentSpeed;
        vel.y = ySpeed;
        controller.Move(vel * Time.deltaTime);
    }

    void StartCrouch()
    {
        if (!isCrouching)
        {
            if (!CheckCeiling())
            {
                isCrouching = true;
                controller.height = crouchHeight;
                controller.center = new Vector3(controller.center.x, crouchingCenterY, controller.center.z);
            }
        }
    }

    void StopCrouch()
    {
        if (isCrouching)
        {
            if (!CheckCeiling())
            {
                isCrouching = false;
                controller.height = originalHeight;
                controller.center = new Vector3(controller.center.x, originalCenterY, controller.center.z);
            }
        }
    }

    void StartGlide()
    {
        if (!isCrouching && !isGliding && !controller.isGrounded)
        {
            isGliding = true;
            glideTimer = 0f; // Réinitialise le chronomètre de planage
        }
    }

    void StopGlide()
    {
        if (isGliding)
        {
            isGliding = false;
        }
    }

    bool CheckCeiling()
    {
        RaycastHit hit;
        float raycastLength = isCrouching ? originalHeight - crouchHeight : originalHeight;

        if (Physics.Raycast(transform.position, Vector3.up, out hit, raycastLength))
        {
            return true;
        }

        return false;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        isInPlayMode = hasFocus;
    }
}

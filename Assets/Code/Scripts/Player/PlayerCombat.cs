using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRadius = 2f;
    public float attackDuration = 1f;
    public LayerMask enemyLayer;

    private bool isAttacking = false;

    // Reference to the PlayerMovement script
    private PlayerMovement playerMovement;

    void Start()
    {
        // Get the PlayerMovement component attached to the same GameObject
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Check if the player is not gliding and not crouching before allowing an attack
        if (Input.GetMouseButtonDown(0) && !isAttacking && !playerMovement.IsGliding && !playerMovement.IsCrouching)
        {
            StartCoroutine(PerformCircularAttack());
        }
    }

    IEnumerator PerformCircularAttack()
    {
        isAttacking = true;
        Debug.Log("Start Attack");

        // Perform the circular attack logic here
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            // You can apply damage or any other logic to the enemies here
            Debug.Log("Hit: " + enemy.name);
        }

        // Wait for the attack duration
        yield return new WaitForSeconds(attackDuration);

        Debug.Log("End Attack");
        isAttacking = false;
    }
}

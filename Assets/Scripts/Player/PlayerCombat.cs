using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRadius = 2f;
    public float attackDuration = 1f;
    public LayerMask enemyLayer;

    private bool isAttacking = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
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

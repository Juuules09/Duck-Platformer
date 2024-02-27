using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRadius = 2f;
    public float attackDuration = 1f;
    public LayerMask enemyLayer;

    private bool isAttacking = false;


    private PlayerMovement playerMovement;

    void Start()
    {
        
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
     
        if (Input.GetMouseButtonDown(0) && !isAttacking && !playerMovement.IsGliding && !playerMovement.IsCrouching)
        {
            StartCoroutine(PerformCircularAttack());
        }
    }

    IEnumerator PerformCircularAttack()
    {
        isAttacking = true;
        Debug.Log("Start Attack");

       
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Hit: " + enemy.name);
        }

        
        yield return new WaitForSeconds(attackDuration);

        Debug.Log("End Attack");
        isAttacking = false;
    }
}

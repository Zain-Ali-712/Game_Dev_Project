// ==========================
// ENEMY HEALTH
// ==========================

using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public int health = 1;

    private bool isDead = false;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeHit(int damage)
    {
        if (isDead) return;

        health -= damage;

        Debug.Log("Enemy Health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        anim.SetTrigger("Die");

        EnemyAI ai = GetComponent<EnemyAI>();

        if (ai != null)
        {
            ai.enabled = false;
        }

        NavMeshAgent agent =
            GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.isStopped = true;
        }
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GetComponent<CharacterController>().enabled = false;
        Collider col = GetComponent<Collider>();

        if (col != null)
        {
            col.enabled = false;
        }

        CharacterController cc =
        GetComponent<CharacterController>();

        if (cc != null)
        {
            cc.enabled = false;
        }

        Destroy(gameObject, 3f);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
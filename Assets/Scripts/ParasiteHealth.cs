// ==========================
// PARASITE HEALTH
// ==========================

using UnityEngine;
using UnityEngine.AI;

public class ParasiteHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 30; // 3 hits to kill (if player damage is 10). Change in Inspector per level!
    private int currentHealth;

    private bool isDead = false;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    public void TakeHit(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        Debug.Log("Parasite Health: " + currentHealth);

        if (currentHealth <= 0)
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

        CharacterController cc =
        GetComponent<CharacterController>();

        if (cc != null)
        {
            cc.enabled = false;
        }

        NavMeshAgent agent =
            GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.isStopped = true;
        }

        Collider col = GetComponent<Collider>();

        if (col != null)
        {
            col.enabled = false;
        }

        Destroy(gameObject, 3f);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
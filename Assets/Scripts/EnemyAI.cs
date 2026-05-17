// ==========================
// ENEMY AI (NAVMESH VERSION) - UI CONNECTED
// ==========================

using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    public float attackRange = 1.5f;

    public bool useAttackIndex = true;

    [Header("Combat Settings")]
    public int damage = 10; // Increased base damage so your health visibly drops faster!

    private Animator anim;
    private NavMeshAgent agent;

    private float attackTimer = 0f;
    private float attackDelay = 2f;

    private int attackIndex = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
    }

    void Update()
    {
        if (player == null) return;

        // Find our global UI LevelManager brain sitting in the scene
        LevelManager lm = Object.FindFirstObjectByType<LevelManager>();

        // If the game is already over (Player Won or Player Lost), freeze this zombie!
        if (lm != null && lm.currentHealth <= 0)
        {
            agent.isStopped = true;
            anim.SetFloat("Speed", 0f);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            AttackPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        anim.SetFloat("Speed", 1f);
    }

    void AttackPlayer()
    {
        agent.isStopped = true;

        transform.LookAt(new Vector3(
            player.position.x,
            transform.position.y,
            player.position.z
        ));

        anim.SetFloat("Speed", 0f);

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDelay)
        {
            attackTimer = 0f;

            if (useAttackIndex)
            {
                anim.SetInteger("AttackIndex", attackIndex);
            }

            anim.SetTrigger("Attack");

            // ROUTE DAMAGE TO YOUR LEVEL MANAGER UI SYSTEM
            LevelManager lm = Object.FindFirstObjectByType<LevelManager>();

            if (lm != null && lm.currentHealth > 0)
            {
                // This forces the health value to drop on screen and checks for death!
                lm.TakeDamage(damage);
            }

            if (useAttackIndex)
            {
                attackIndex++;

                if (attackIndex > 1)
                {
                    attackIndex = 0;
                }
            }
        }
    }
}
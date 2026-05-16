// ==========================
// ENEMY AI (NAVMESH VERSION)
// ==========================

using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    public float attackRange = 1.5f;

    public bool useAttackIndex = true;

    public int damage = 1;

    private Animator anim;
    private NavMeshAgent agent;

    private float attackTimer = 0f;
    private float attackDelay = 2f;

    private int attackIndex = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        PlayerHealth ph = player.GetComponent<PlayerHealth>();

        if (ph != null && ph.IsDead())
        {
            agent.isStopped = true;
            anim.SetFloat("Speed", 0f);
            return;
        }

        float distance =
            Vector3.Distance(transform.position, player.position);

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

            PlayerHealth ph =
                player.GetComponent<PlayerHealth>();

            if (ph != null && !ph.IsDead())
            {
                ph.TakeHit(damage);
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
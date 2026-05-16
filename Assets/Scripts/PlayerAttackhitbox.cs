using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    [Header("Combat Settings")]
    public int attackDamage = 10; // 10 damage per hit. Change per level in Inspector!

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("COLLIDER HIT: " + other.name);

        Transform current = other.transform;

        while (current != null)
        {
            Debug.Log("PARENT: " + current.name);

            current = current.parent;
        }

        if (!other.CompareTag("Enemy"))
            return;

        EnemyHealth enemy1 =
            other.GetComponentInParent<EnemyHealth>();

        ParasiteHealth enemy2 =
            other.GetComponentInParent<ParasiteHealth>();

        Debug.Log("EnemyHealth Found: " + (enemy1 != null));
        Debug.Log("ParasiteHealth Found: " + (enemy2 != null));

        if (enemy1 != null && !enemy1.IsDead())
        {
            enemy1.TakeHit(attackDamage);
            Debug.Log("Enemy Hit! Dealt " + attackDamage + " damage.");
        }

        if (enemy2 != null && !enemy2.IsDead())
        {
            enemy2.TakeHit(attackDamage);
            Debug.Log("Parasite Hit! Dealt " + attackDamage + " damage.");
        }
    }
}
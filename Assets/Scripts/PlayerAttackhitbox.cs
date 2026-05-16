using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
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
            enemy1.TakeHit(1);
            Debug.Log("Enemy Hit!");
        }

        if (enemy2 != null && !enemy2.IsDead())
        {
            enemy2.TakeHit(1);
            Debug.Log("Parasite Hit!");
        }
    }
}
// ==========================
// PLAYER HEALTH
// ==========================

using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 10;

    private Animator anim;
    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeHit(int damage)
    {
        if (isDead) return;

        health -= damage;

        Debug.Log("Player Health: " + health);

        anim.SetTrigger("Hit");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        anim.SetTrigger("Die");

        GetComponent<PlayerController>().enabled = false;

        CharacterController controller =
            GetComponent<CharacterController>();

        GetComponent<CharacterController>().enabled = false;


        if (controller != null)
        {
            controller.enabled = false;
        }

        Debug.Log("PLAYER DEAD");
    }

    public bool IsDead()
    {
        return isDead;
    }
}
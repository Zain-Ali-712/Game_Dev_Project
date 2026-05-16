using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 10f;
    public float gravity = -20f;

    [Header("Attack")]
    public GameObject attackpoint;

    private Animator anim;
    private CharacterController controller;

    private Vector3 velocity;
    private Vector3 moveDirection;

    // Combo System
    private int attackIndex = 0;
    private float comboTimer = 0f;
    private float comboDelay = 2f;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    // ================= MOVEMENT =================
    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            // Move
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Rotate
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            anim.SetFloat("Speed", 1f);
        }
        else
        {
            anim.SetFloat("Speed", 0f);
        }

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // ================= ATTACK =================
    void HandleAttack()
    {
        comboTimer += Time.deltaTime;

        if (comboTimer > comboDelay)
        {
            attackIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        comboTimer = 0f;

        anim.SetInteger("AttackIndex", attackIndex);
        anim.SetTrigger("Attack");

        StartCoroutine(AttackRoutine());

        attackIndex++;

        if (attackIndex > 2)
        {
            attackIndex = 0;
        }
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        attackpoint.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        attackpoint.SetActive(false);
    }
}
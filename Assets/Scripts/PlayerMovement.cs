using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private PlayerController playerController;

    private float mobileInputX = 0f;
    private Vector2 moveInput;
    private bool isJumping = false;

    private enum MovementState { idle, walk, jump, fall, run, death }
    private bool isDead = false;
    private bool hasDiedAnimPlayed = false;

    [Header("Jump Settings")]
    [SerializeField] private LayerMask jumpableGround;
    private BoxCollider2D coll;
    private int jumpCount = 0;
    [SerializeField] private int maxJumps = 2;

    [Header("Knockback Settings")]
    [SerializeField] private float knockBackTime = 0.2f;
    [SerializeField] private float knockBackThrust = 10f;
    private bool isKnockedBack = false;

    [Header("Health System")]
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthBar;

    private Vector3 startPoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();

        playerController = new PlayerController();
        currentHealth = maxHealth;
        UpdateHealthUI();
        hasDiedAnimPlayed = false;
    }
    private void Start()
    {
        AudioManager.instance.PlayBackgroundMusic();
        startPoint = transform.position;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

    }


    private void OnEnable()
    {
        playerController.Enable();

        playerController.Movement.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerController.Movement.Move.canceled += ctx => moveInput = Vector2.zero;

        playerController.Movement.Jump.performed += ctx => Jump();
    }

    private void OnDisable()
    {
        playerController.Disable();
    }

    private void Update()
    {
        if (isDead) return;

        if (Application.isMobilePlatform)
        {
            moveInput = new Vector2(mobileInputX, 0f);
        }
        else
        {
            moveInput = playerController.Movement.Move.ReadValue<Vector2>();
        }
    }

    private void FixedUpdate()
    {
        if (isDead || isKnockedBack) return;

        Vector2 targetVelocity = new Vector2((moveInput.x + mobileInputX) * moveSpeed, rb.velocity.y);
        rb.velocity = targetVelocity;

        // 🔊 Suara jalan jika bergerak di tanah
        if (isGrounded() && Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            AudioManager.instance.PlaySound("walk");
        }

        UpdateAnimation();

        if (isGrounded() && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            isJumping = false;
            jumpCount = 0; // Reset jump count when grounded
        }
    }

    private void UpdateAnimation()
    {
        if (isDead)
        {
            if (!hasDiedAnimPlayed)
            {
                anim.SetInteger("state", (int)MovementState.death);
                hasDiedAnimPlayed = true;
            }
            return;
        }

        MovementState state;
        float horizontal = moveInput.x != 0 ? moveInput.x : mobileInputX;

        if (horizontal > 0f)
        {
            state = MovementState.walk;
            sprite.flipX = false;

            PlayWalkSound(); // ⬅️ Tambahkan ini
        }
        else if (horizontal < 0f)
        {
            state = MovementState.walk;
            sprite.flipX = true;

            PlayWalkSound(); // ⬅️ Tambahkan ini
        }
        else
        {
            state = MovementState.idle;
            StopWalkSound(); // ⬅️ Tambahkan ini juga
        }

        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jump;
            StopWalkSound(); // hentikan suara walk saat loncat
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.fall;
            StopWalkSound(); // hentikan suara walk saat jatuh
        }

        anim.SetInteger("state", (int)state);
    }

    private void PlayWalkSound()
    {
        if (!AudioManager.instance.walkSource.isPlaying)
        {
            AudioManager.instance.walkSource.Play();
        }
    }

    private void StopWalkSound()
    {
        if (AudioManager.instance.walkSource.isPlaying)
        {
            AudioManager.instance.walkSource.Stop();
        }
    }


    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void Jump()
    {
        if (isDead) return;

        if (jumpCount < maxJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
            jumpCount++;

            // 🔊 Suara lompat
            AudioManager.instance.PlaySound("jump");
        }
    }

    public void MoveRight(bool isPressed)
    {
        if (isDead) return;

        if (isPressed)
            mobileInputX = 1f;
        else if (mobileInputX == 1f)
            mobileInputX = 0f;
    }

    public void MoveLeft(bool isPressed)
    {
        if (isDead) return;

        if (isPressed)
            mobileInputX = -1f;
        else if (mobileInputX == -1f)
            mobileInputX = 0f;
    }

    public void MobileJump()
    {
        if (isDead) return;

        Jump();
    }

    public void TakeDamage(int damage, Vector2 direction)
    {
        if (isKnockedBack || isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            return;
        }

        StartCoroutine(HandleKnockback(direction.normalized));
        UpdateHealthUI();
    }

    private void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;

        anim.SetInteger("state", (int)MovementState.death);
        hasDiedAnimPlayed = true;

        // 🔊 Suara mati
        AudioManager.instance.PlaySound("died");

        UpdateHealthUI();
        StartCoroutine(RestartLevel());
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
            healthBar.value = currentHealth;
    }


    private IEnumerator HandleKnockback(Vector2 direction)
    {
        isKnockedBack = true;
        rb.velocity = Vector2.zero;

        Vector2 force = direction * knockBackThrust * rb.mass;
        rb.AddForce(force, ForceMode2D.Impulse);

        // 🔊 Suara kena trap / knockback
        AudioManager.instance.PlaySound("knockback");

        yield return new WaitForSeconds(knockBackTime);
        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }
    public void ResetToStartPoint()
    {
        rb.velocity = Vector2.zero;
        transform.position = startPoint;
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (QuestionManager.Instance != null)
        {
            QuestionManager.Instance.ResetScore();
        }

        isDead = false;
        hasDiedAnimPlayed = false;
    }

}
